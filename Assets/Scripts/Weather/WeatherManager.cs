using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe.Weather
{
    public class WeatherManager : MonoBehaviour
    {
        public PrecipitationChance[] Precipitations;  // NOT mutually exclusive
        public Chance WindDirection, WindIntensity, Rainbow, Clouds; // Mutually exclusive events
        public Color RainColor = new Color(0, .5f, 1);

        public const float WeatherReset = 10 * 60;
        public const int widthOfRenderer = 100;
        private static Dictionary<int, WeatherRenderer> weatherRenderers;
        public static WeatherManager Instance { get; private set; }
        private int currentWeatherResetTick;

        [SerializeField]
        private float height = 20;

        [SerializeField]
        private WeatherRenderer weatherRendererPrefab;

        [SerializeField]
        private Gradient lightning;

        [HideInInspector]
        public Color lightningColor;

        private void Awake()
        {
            Instance = this;
            weatherRenderers = new Dictionary<int, WeatherRenderer>();
        }

        private void Start()
        {
            CameraControl.Instance.OnPositionUpdate += TrySpawnWeatherObject;

            if (BodyManager.Parent is Planet p)
                RainColor = p.waterColor;
            else if (BodyManager.Parent is Star star)
            {
                RainColor = star.StarColor;
                lightningColor = star.StarColor;
                return;
            }

            System.Random rand = new(BodyManager.GetSeed());
            RandomNum.Init(rand);

            lightningColor = lightning.Evaluate((float)rand.NextDouble());
        }

        private void Update()
        {
            int weatherResetTick = (int)(GlobalTime.Time / WeatherReset);
            if (weatherResetTick == currentWeatherResetTick)
                return;
            currentWeatherResetTick = weatherResetTick;
            SetWeatherTick(currentWeatherResetTick);
        }

        private void SetWeatherTick(int tick)
        {
            foreach (var weatherRenderer in weatherRenderers.Values)
            {
                weatherRenderer.ChangeWeather(tick);
            }
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= TrySpawnWeatherObject;
        }

        private void TrySpawnWeatherObject(Rect bounds)
        {
            const int halfWidth = widthOfRenderer >> 1;
            int min = (int)Mathf.Floor(bounds.xMin / widthOfRenderer) * widthOfRenderer - halfWidth;
            int max = (int)Mathf.Ceil(bounds.xMax / widthOfRenderer) * widthOfRenderer + halfWidth;

            var list = weatherRenderers.ToList();

            // remove out of range

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Key >= min && list[i].Key <= max)
                    continue;
                // out of range
                Destroy(list[i].Value.gameObject);
                weatherRenderers.Remove(list[i].Key);
            }

            // add not yet in range

            for (int i = min; i < max; i += widthOfRenderer)
            {
                if (weatherRenderers.ContainsKey(i))
                    continue; // already exists
                weatherRenderers.Add(i, SpawnWeather(i));
            }
        }

        private WeatherRenderer SpawnWeather(int xPos)
        {
            var weatherRenderer = Instantiate(weatherRendererPrefab);
            int seed = new Vector2(xPos, xPos).HashPos(BodyManager.GetSeed());
            weatherRenderer.Spawn(new Vector3(xPos, height + (seed % 10) - 5), seed);
            weatherRenderer.ChangeWeather(currentWeatherResetTick);
            return weatherRenderer;
        }

        [System.Serializable]
        public struct Chance
        {
            public float Probability, Average, Deviation;

            public Chance(float probability, float average, float deviation)
            {
                Probability = probability;
                Average = average;
                Deviation = deviation;
            }

            public Chance(float probability)
            {
                Probability = probability;
                Average = 0;
                Deviation = 0;
            }
        }

        [System.Serializable]
        public struct PrecipitationChance
        {
            public Precipitation Type;
            public Chance chance;

            public PrecipitationChance(Precipitation type, Chance chance)
            {
                Type = type;
                this.chance = chance;
            }
        }
    }
}
