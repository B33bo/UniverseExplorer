using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class GasPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private ParticleSystem[] particles;

        [SerializeField]
        private ParticleSystem grey;

        public override Type PlanetType => typeof(GasPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Radius, (float)GasPlanet.MinScale, (float)GasPlanet.MaxScale) * 3 * Vector2.one;

            Target.OnInspected += RegenColors;
            RegenColors(null);
        }

        private void RegenColors(Variable variable)
        {
            ColorHSV gasColor = (Target as GasPlanet).GasColor;
            float contrast = (Target as GasPlanet).Contrast;
            System.Random rand = new System.Random(Target.Seed);

            sprite.color = gasColor;

            for (int i = 0; i < particles.Length; i++)
            {
                var particleSystem = particles[i].main;

                ColorHSV color = gasColor;
                color.h += RandomNum.GetFloat(-contrast, contrast, rand);
                color.s += RandomNum.GetFloat(-contrast, contrast, rand) * GasPlanet.ContrastSatDropoff;
                color.v += RandomNum.GetFloat(-contrast, contrast, rand) * GasPlanet.ContrastSatDropoff;
                particleSystem.startColor = new ParticleSystem.MinMaxGradient(color);
            }
        }

        protected override void HighRes()
        {
            base.HighRes();
            for (int i = 0; i < particles.Length; i++)
                particles[i].Play();
            grey.Play();
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            base.LowRes();
            for (int i = 0; i < particles.Length; i++)
                particles[i].Stop();
            grey.Stop();
        }
    }
}
