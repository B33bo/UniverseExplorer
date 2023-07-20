using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class GasPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private ParticleSystem[] particles;

        [SerializeField]
        private SpriteRenderer body;

        public override Type PlanetType => typeof(GasPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Radius, (float)GasPlanet.MinScale, (float)GasPlanet.MaxScale) * 3 * Vector2.one;

            RegenColors();
            (Target as GasPlanet).OnColorChange += RegenColors;
        }

        private void RegenColors()
        {
            ColorHSV gasColor = (Target as GasPlanet).GasColor;
            float contrast = (Target as GasPlanet).Contrast;
            System.Random rand = new System.Random(Target.Seed);

            body.color = gasColor;

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
    }
}
