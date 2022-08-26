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
                OverrideRockColor = Color.HSVToRGB(rockyPlanet.RockColor.H, rockyPlanet.RockColor.S, rockyPlanet.RockColor.V);
                SkyDayColor = Color.HSVToRGB(rockyPlanet.RockColor.H, rockyPlanet.RockColor.S - .1f, rockyPlanet.RockColor.V - .2f);
                SkyNightColor = Color.HSVToRGB(rockyPlanet.RockColor.H, rockyPlanet.RockColor.S - .1f, rockyPlanet.RockColor.V - .5f);
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
            Vector2 position = CameraControl.Instance.transform.position;

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

        private float GetY(float x, float maxBlockHeight)
        {
            if (x % 10 == 0)
            {
                var rnd = new System.Random(new System.Random((int)(x * 300)).Next() + seed + seedOffset);
                if (x < 0)
                    rnd.NextDouble();
                return (float)rnd.NextDouble() * maxBlockHeight;
            }

            float left = Mathf.Floor(x / 10) * 10;
            float right = Mathf.Ceil(x / 10) * 10;

            float t = (x - left) / (right - left);

            return Mathf.Lerp(GetY(left, maxBlockHeight), GetY(right, maxBlockHeight), t);
        }

        private IEnumerable<float> GetPositions(float xMin, float xMax)
        {
            for (float x = xMin; x < xMax; x += QuadrantSize)
                yield return x;
        }

        public Biome BiomeAtPosition(float xPos)
        {
            xPos = Mathf.Floor(xPos / BiomeSize) * BiomeSize;
            if (cachedBiomes.TryGetValue((int)xPos, out Biome value))
                return value;

            System.Random rnd = new System.Random(seed * (int)xPos);

            float[] biomeWeights = new float[] { 1 };
            Biome[] biomes = new Biome[] { defaultBiome };

            if (Mathf.Approximately(xPos, 0))
            {
                biomeWeights = new float[] { 1 };
                biomes = new Biome[] { defaultBiome };
            }

            if (xPos > 0)
            {
                Biome prevBiome = BiomeAtPosition(xPos - BiomeSize);
                if (prevBiome)
                {
                    biomeWeights = prevBiome.biomeWeights;
                    biomes = prevBiome.adjacentBiomes;
                }
            }

            if (xPos < 0)
            {
                Biome prevBiome = BiomeAtPosition(xPos + BiomeSize);
                if (prevBiome)
                {
                    biomeWeights = prevBiome.biomeWeights;
                    biomes = prevBiome.adjacentBiomes;
                }
            }

            Biome b = biomes[RandomNum.GetIndexFromWeights(biomeWeights, rnd)];
            cachedBiomes.Add((int)xPos, b);
            return b;
        }

        private CelestialBodyRenderer GetRandomObject(Vector2 position, Biome biome)
        {
            System.Random randFromPos = new System.Random(((int)(position.x * position.x) * (int)Mathf.Sign(position.x) + seed) * 4);
            if (biome.objects.Length == 0)
                return null;
            return biome.objects[RandomNum.GetIndexFromWeights(biome.weights, randFromPos)];
        }
    }
}
