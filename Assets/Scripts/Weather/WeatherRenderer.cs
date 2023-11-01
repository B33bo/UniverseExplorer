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

        public override void Spawn(Vector2 pos, int? seed)
        {
            Weather weather = new Weather();
            Target = weather;
            if (seed.HasValue)
                weather.SetSeed(seed.Value);
            weather.Create(pos);

            var rainMain = rain.main;
            rainMain.startColor = rainMain.startColor.color * WeatherManager.Instance.RainColor;

            rainbow.Spawn(new Vector2(0, -pos.y + 2), seed);
            Debug.Log(rainbow.Target.Position);
        }

        public void ChangeWeather(int weatherIndex)
        {
            var weather = Target as Weather;

            var oldParticles = GetPrecipitationParticles(weather.PrecipitationType);

            if (oldParticles != null)
                oldParticles.Stop(true);

            weather.WeatherChange(WeatherManager.Instance, weatherIndex);

            ChangePrecipitation(weather);

            rainbow.gameObject.SetActive(weather.RainbowIntensity != 0);
            rainbow.SetIntensity(weather.RainbowIntensity);

            Target.Name = weather.PrecipitationType.ToString();
        }

        private void ChangePrecipitation(Weather weather)
        {
            var newParticles = GetPrecipitationParticles(weather.PrecipitationType);

            if (newParticles == null)
                return;

            newParticles.Play(true);

            var emission = newParticles.emission;
            emission.rateOverTime = weather.PrecipitationIntensity;

            var main = newParticles.main;
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
                main.startRotation = weather.WindDirection * Mathf.Deg2Rad;

            var velocity = newParticles.velocityOverLifetime;
            velocity.x = -Mathf.Cos(weather.WindDirection * Mathf.Deg2Rad) * weather.WindIntensity;
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
