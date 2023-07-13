using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class GasPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private ParticleSystem[] particles;

        public override Type PlanetType => typeof(GasPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Radius, (float)GasPlanet.MinScale, (float)GasPlanet.MaxScale) * 3 * Vector2.one;
            for (int i = 0; i < particles.Length; i++)
            {
                var particleSystem = particles[i].main;
                particles[i].randomSeed = (uint)Target.RandomNumberGenerator.Next();
                particles[i].Play();
                Color c = RandomNum.GetColor(20, 50, Target.RandomNumberGenerator);
                particleSystem.startColor = new ParticleSystem.MinMaxGradient(c);
            }
        }
    }
}
