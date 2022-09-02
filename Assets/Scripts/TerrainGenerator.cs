using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class TerrainGenerator : MonoBehaviour
    {
        public const float BiomeSize = 200;
        public static TerrainGenerator Instance { get; private set; }

        private const float QuadrantSize = .5f;
        private Dictionary<float, (SpriteRenderer tile, CelestialBodyRenderer obj)> loadedPositions;
        private int seed;

        [SerializeField]
        private float YOffset;

        [SerializeField]
        private int seedOffset = 0;

        [SerializeField]
        private bool ColorSky;

        [SerializeField]
        private Color SkyDayColor, SkyNightColor;

        [SerializeField]
        private DayNightSystem dayNightSystem;

        [SerializeField]
        private bool doEssentialFunctions = true;

        [SerializeField]
        private Biome defaultBiome;

        private Dictionary<int, Biome> cachedBiomes = new Dictionary<int, Biome>();

        public static Color? OverrideRockColor = null;

        private IEnumerator Start()
        {
            Instance = this;
            if (BodyManager.Parent is RockyPlanet rockyPlanet)
            {
                OverrideRockColor = rockyPlanet.RockColor;

                SkyDayColor = new ColorHSV(rockyPlanet.RockColor.h, rockyPlanet.RockColor.s - .1f, rockyPlanet.RockColor.v - .2f);
                SkyNightColor = new ColorHSV(rockyPlanet.RockColor.h, rockyPlanet.RockColor.s - .1f, rockyPlanet.RockColor.v - .5f);
            }
            else if (BodyManager.Parent is Star star)
            {
                OverrideRockColor = star.starColor;
                SkyDayColor = star.starColor;
                SkyNightColor = star.starColor;
            }

            yield return new WaitForFrames(1);

            if (doEssentialFunctions)
            {
                BodyManager.ReloadCommands();
                BodyManager.InvokeSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }

            if (BodyManager.Parent is null)
            {
                BodyManager.Parent = new ErrorPlanet();
                BodyManager.Parent.SetSeed(1);
                BodyManager.Parent.Create(Vector2.zero);
            }

            seed = BodyManager.GetSeed();
            loadedPositions = new Dictionary<float, (SpriteRenderer, CelestialBodyRenderer)>();

            ReloadBlocks(CameraControl.Instance.CameraBounds);
            CameraControl.Instance.OnPositionUpdate += CameraUpdate;

            Btools.DevConsole.DevCommands.Register("findbiome", "finds the biome you specify", FindBiome, "biome");
        }

        private string FindBiome(string[] args)
        {
            float currentPos = CameraControl.Instance.Position.x;

            int maxAttempts = 5000;

            if (args.Length > 2)
                maxAttempts = int.Parse(args[2]);

            int attempts = 0;
            while (++attempts < maxAttempts)
            {
                if (BiomeAtPosition(currentPos).name.ToLower() == args[1].ToLower())
                    return currentPos.ToString();
                if (BiomeAtPosition(-currentPos).name.ToLower() == args[1].ToLower())
                    return (-currentPos).ToString();
                currentPos += BiomeSize;
            }

            return $"could not find biome {args[1]}";
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= CameraUpdate;
        }

        private void CameraUpdate(Rect bounds)
        {
            ReloadBlocks(bounds);
        }

        private void Update()
        {
            if (ColorSky)
            {
                CameraControl.Instance.MyCamera.backgroundColor = GetCameraColor();
            }
        }

        private Color GetCameraColor()
        {
            if (dayNightSystem is null)
                return SkyDayColor;

            if (dayNightSystem.Time > 1)
                return Color.Lerp(SkyNightColor, SkyDayColor, dayNightSystem.Time - 1);
            return Color.Lerp(SkyDayColor, SkyNightColor, dayNightSystem.Time);
        }

        private void ReloadBlocks(Rect cameraBounds)
        {
            float xMin = cameraBounds.xMin - 5;
            float xMax = cameraBounds.xMax + 5;

            xMin = Mathf.Round(xMin * QuadrantSize) / QuadrantSize;
            xMax = Mathf.Round(xMax * QuadrantSize) / QuadrantSize;

            var positions = GetPositions(xMin, xMax);

            float[] oldPositions = loadedPositions.Keys.ToArray();

            //Delete unused positions
            for (int i = 0; i < oldPositions.Length; i++)
            {
                if (oldPositions[i] < xMin || oldPositions[i] > xMax)
                {
                    var (tile, obj) = loadedPositions[oldPositions[i]];
                    if (!(obj is null))
                        Destroy(obj.gameObject);
                    Destroy(tile.gameObject);
                    loadedPositions.Remove(oldPositions[i]);
                    continue;
                }
            }
            
            foreach (float pos in positions)
            {
                if (loadedPositions.ContainsKey(pos))
                    continue;

                Biome biome = BiomeAtPosition(pos);
                float Ypos = GetY(pos, biome.MaxHeight);
                loadedPositions.Add(pos, (SpawnBlockAt(new Vector2(pos, Ypos), biome), SpawnObjectAt(new Vector2(pos, Ypos * 2), biome)));
            }
        }

        private SpriteRenderer SpawnBlockAt(Vector2 position, Biome biome)
        {
            var newBlock = Instantiate(biome.groundPrefab, position + new Vector2(0, YOffset), Quaternion.identity);

            if (biome.colorGround)
                newBlock.color = biome.groundColor;

            if (BodyManager.Parent is RockyPlanet rockyPlanet)
                newBlock.color = rockyPlanet.RockColor;

            if (newBlock.drawMode == SpriteDrawMode.Simple)
                newBlock.transform.localScale = new Vector3(newBlock.transform.localScale.x, position.y * 2);
            else
                newBlock.size = new Vector2(newBlock.size.x, position.y * 2);
            return newBlock;
        }

        private CelestialBodyRenderer SpawnObjectAt(Vector2 pos, Biome biome)
        {
            Vector2 positionVector = pos;
            var target = GetRandomObject(positionVector, biome);
            if (target is null)
                return null;

            CelestialBodyRenderer newObject = Instantiate(target, positionVector, Quaternion.identity);
            newObject.Spawn(positionVector, null);
            return newObject;
        }

        private float GetY(float x, float maxBlockHeight, int iterations = 0)
        {
            if (iterations > 1000)
                throw new System.Exception("too many iterations");
            if (x % 10 == 0)
            {
                var rnd = new System.Random(new System.Random((int)(x * 300)).Next() + seed + seedOffset);
                if (x < 0)
                    rnd.NextDouble();
                return (float)rnd.NextDouble() * maxBlockHeight;
            }

            float left = Mathf.Floor(x / 10) * 10;
            float right = Mathf.Ceil(x / 10) * 10;

            if (left == x || right == x || left == right)
            {
                Debug.LogError("Ground cannot find correct Y level, defaulting to 0");
                return 0;
            }

            float t = (x - left) / (right - left);

            return Mathf.Lerp(GetY(left, maxBlockHeight, iterations), GetY(right, maxBlockHeight, iterations), t);
        }

        private IEnumerable<float> GetPositions(float xMin, float xMax)
        {
            float previousX = float.NaN;
            for (float x = xMin; x < xMax; x += QuadrantSize)
            {
                //for extremely high positions (>8388532)
                if (previousX == x)
                {
                    Debug.LogError("you are out of range of valid terrain generation");
                    yield break;
                }

                yield return x;
                previousX = x;
            }
        }

        public Biome BiomeAtPosition(float xPos, int depth = 0)
        {
            xPos = Mathf.Floor(xPos / BiomeSize) * BiomeSize;
            if (cachedBiomes.TryGetValue((int)xPos, out Biome value))
                return value;

            if (depth > 1000)
                throw new System.Exception("Depth exceeded 10_000");

            System.Random rnd = new System.Random((int)xPos * seed + 123421345);

            float[] biomeWeights = new float[] { 1 };
            Biome[] biomes = new Biome[] { defaultBiome };

            if (!((int)xPos % 5000 == 0 || xPos == xPos + 1))
            {
                if (Mathf.Approximately(xPos, 0))
                {
                    biomeWeights = new float[] { 1 };
                    biomes = new Biome[] { defaultBiome };
                }

                if (xPos > 0)
                {
                    Debug.Log("finding left of " + xPos);
                    Biome previous = BiomeAtPosition(xPos - BiomeSize, depth + 1);
                    biomeWeights = previous.biomeWeights;
                    biomes = previous.adjacentBiomes;
                }

                if (xPos < 0)
                {
                    Debug.Log("finding right of " + xPos);
                    Biome next = BiomeAtPosition(xPos + BiomeSize, depth + 1);
                    biomeWeights = next.biomeWeights;
                    biomes = next.adjacentBiomes;
                }
            }

            int index = RandomNum.GetIndexFromWeights(biomeWeights, rnd, out float randomValue);
            Biome b = biomes[index];

            if (cachedBiomes.Count >= 200)
                cachedBiomes.Clear();

            cachedBiomes.Add((int)xPos, b);
            Debug.Log($"Generated new biome {b.name}, x = {(int)xPos}");
            return b;
        }

        private CelestialBodyRenderer GetRandomObject(Vector2 position, Biome biome)
        {
            System.Random randFromPos = new System.Random(((int)(position.x * position.x) * (int)Mathf.Sign(position.x) + seed) * 4);
            if (biome.objects.Length == 0)
                return null;

#if UNITY_EDITOR
            if (biome.weights.Length != biome.objects.Length || biome.objects.Length == 0)
                Debug.LogError("UH OH");
#endif

            int index = RandomNum.GetIndexFromWeights(biome.weights, randFromPos);
            return biome.objects[index];
        }
    }
}
