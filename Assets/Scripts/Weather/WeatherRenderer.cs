using UnityEngine;
using Universe.CelestialBodies;

namespace Universe.Weather
{
    public class WeatherRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ParticleSystem rain, snow, hail, sand, meteor, shootingStars;

        [SerializeField]
        private RainbowRenderer rainbow;

        [SerializeField]
        private CloudRenderer cloud;

        [SerializeField]
        private LightningRendrerer lightning;
        private Weather weather;
        private float timeOfLastThunder;

        public override void Spawn(Vector2 pos, int? seed)
        {
            weather = new();
            Target = weather;
            if (seed.HasValue)
                weather.SetSeed(seed.Value);
            weather.Create(pos);

            var rainMain = rain.main;
            rainMain.startColor = rainMain.startColor.color * WeatherManager.Instance.RainColor;

            rainbow.Spawn(new Vector2(0, -pos.y + 2), seed);

            cloud.Spawn(new Vector2(0, 0), seed);
        }

        public void ChangeWeather(int weatherIndex)
        {
            var oldParticles = GetPrecipitationParticles(weather.PrecipitationType);

            if (oldParticles != null)
                oldParticles.Stop(true);

            weather.WeatherChange(WeatherManager.Instance, weatherIndex);

            ChangePrecipitation();

            rainbow.gameObject.SetActive(weather.RainbowIntensity != 0);
            rainbow.SetIntensity(weather.RainbowIntensity);

            Target.Name = weather.PrecipitationType.ToString();
        }

        private void ChangePrecipitation()
        {
            var newParticles = GetPrecipitationParticles(weather.PrecipitationType);

            if (newParticles == null)
            {
                cloud.Dissapear();
                return;
            }

            cloud.Appear();

            newParticles.Play(true);

            var emission = newParticles.emission;
            emission.rateOverTime = weather.PrecipitationIntensity;

            var main = newParticles.main;
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
                main.startRotation = weather.WindDirection * Mathf.Deg2Rad;

            var velocity = newParticles.velocityOverLifetime;
            velocity.x = -Mathf.Cos(weather.WindDirection * Mathf.Deg2Rad) * weather.WindIntensity;
        }

        public override void OnUpdate()
        {
            if (weather.PrecipitationType == Precipitation.Thunder)
                TryLightning();
        }

        private void TryLightning()
        {
            float frequencyOfThunder = 125f / weather.PrecipitationIntensity;

            if (Time.time - timeOfLastThunder < frequencyOfThunder)
                return;
            timeOfLastThunder = Time.time;
            int seed = Mathf.FloorToInt(Time.time / frequencyOfThunder) ^ Target.Seed;
            System.Random rand = new(seed);

            float probability = (.2f / 125f) * weather.PrecipitationIntensity;

            if (RandomNum.GetBool(probability, rand))
                Lightning(rand);
        }

        private void Lightning(System.Random rand)
        {
            float xPos = RandomNum.GetFloat(WeatherManager.widthOfRenderer, rand) - WeatherManager.widthOfRenderer * .5f;
            LightningRendrerer newLightning = Instantiate(lightning);

            newLightning.color = WeatherManager.Instance.lightningColor;
            newLightning.Spawn(Target.Position + new Vector3(xPos, 0), rand.Next());
            Destroy(newLightning.gameObject, 10f);
        }

        private ParticleSystem GetPrecipitationParticles(Precipitation precipitation)
        {
            return precipitation switch
            {
                Precipitation.Rain => rain,
                Precipitation.Thunder => rain,
                Precipitation.Snow => snow,
                Precipitation.Hail => hail,
                Precipitation.Sandstorm => sand,
                Precipitation.Meteor => meteor,
                Precipitation.ShootingStars => shootingStars,
                _ => null,
            };
        }
    }
}
