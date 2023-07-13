using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class StarGenerator : MonoBehaviour
    {
        public const float QuadrantSize = .25f;

        private static Star star;
        private int seed;

        [SerializeField]
        private GameObject starParticles;

        [SerializeField]
        private SpriteRenderer ground;

        private static Dictionary<float, GameObject> loadedPositions = new Dictionary<float, GameObject>();

        private Color minStarColor, maxStarColor;

        private IEnumerator Start()
        {
            yield return new WaitForFrames(1);

            BodyManager.ReloadCommands();
            BodyManager.RegisterSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

            if (BodyManager.Parent is Star newStar)
            {
                star = newStar;
                seed = newStar.Seed;
            }
            else
            {
                star = new Star();
                star.Create(Vector2.zero);
                Gradient temperatureGradient = new Gradient();

                temperatureGradient.SetKeys(new GradientColorKey[]
                {
                    new GradientColorKey(new Color(.79f, 0, 0), 0),
                    new GradientColorKey(new Color(1, 1, .25f), .16f),
                    new GradientColorKey(new Color(1, 1, 1), .37f),
                    new GradientColorKey(new Color(.48f, .87f, 1), .53f),
                    new GradientColorKey(new Color(.21f, .53f, .76f), 1),
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1, 0),
                    new GradientAlphaKey(1, 1),
                });

                star.starColor = temperatureGradient.Evaluate((float)(star.Temperature - 3000) / 7000f);
                seed = 0;
            }

            Color.RGBToHSV(star.starColor, out float H, out float S, out float V);

            minStarColor = Color.HSVToRGB(H - .1f, S - .2f, V - .2f);
            maxStarColor = Color.HSVToRGB(H + .1f, S + .2f, V + .2f);

            ground.color = star.starColor;
            ReloadBlocks(CameraControl.Instance.CameraBounds);
            CameraControl.Instance.OnPositionUpdate += ReloadBlocks;
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= ReloadBlocks;
        }

        private void ReloadBlocks(Rect cameraBounds)
        {
            float xMin = cameraBounds.xMin - 5;
            float xMax = cameraBounds.xMax + 5;
            xMin = Mathf.Round(xMin * QuadrantSize) / QuadrantSize;
            xMax = Mathf.Round(xMax * QuadrantSize) / QuadrantSize;
            var positions = GetPositions(xMin, xMax);
            float[] oldPositions = loadedPositions.Keys.ToArray();

            for (int i = 0; i < oldPositions.Length; i++)
            {
                if (oldPositions[i] < xMin || oldPositions[i] > xMax)
                {
                    var tile = loadedPositions[oldPositions[i]];
                    Destroy(tile.gameObject);
                    loadedPositions.Remove(oldPositions[i]);
                    continue;
                }
            }

            foreach (float pos in positions)
            {
                if (loadedPositions.ContainsKey(pos))
                    continue;

                float Ypos = GetY(pos);
                loadedPositions.Add(pos, SpawnBlockAt(pos, Ypos));
            }
        }

        private GameObject SpawnBlockAt(float x, float y)
        {
            var newBlock = Instantiate(starParticles, new Vector3(x, 0), Quaternion.identity);
            ParticleSystem particles = newBlock.GetComponentInChildren<ParticleSystem>();
            var main = particles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(minStarColor, maxStarColor);
            particles.randomSeed = (uint)(x * 1000);
            main.startLifetime = y * 3;
            particles.Play();

            return newBlock;
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

        private float GetY(float x)
        {
            if (x % 10 == 0)
            {
                var rnd = new System.Random(new System.Random((int)(x * 300)).Next() + seed);
                if (x < 0)
                    rnd.NextDouble();
                return (float)rnd.NextDouble();
            }

            float left = Mathf.Floor(x / 10) * 10;
            float right = Mathf.Ceil(x / 10) * 10;

            if (left == right || left == x || right == x)
            {
                Debug.LogError("Ground cannot find correct Y level, defaulting to 0");
                return 0;
            }

            float t = (x - left) / (right - left);

            return Mathf.Lerp(GetY(left), GetY(right), t);
        }
    }
}
