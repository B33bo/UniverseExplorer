using System;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public abstract class PlanetRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Orbiter orbiter;

        public override void Spawn(Vector2 pos, int? seed)
        {
            bool activateOrbiter = false;

            if (BodyManager.Parent is null || BodyManager.Parent.GetType() != PlanetType)
            {
                Target = (CelestialBody)Activator.CreateInstance(PlanetType);
                if (seed.HasValue)
                    Target.SetSeed(seed.Value);
                Target.Create(pos);

                if (BodyManager.Parent is Moon)
                    Destroy(orbiter);
                else
                    activateOrbiter = true;
            }
            else
            {
                Target = BodyManager.Parent;
                Target.SetSeed(Target.Seed);
                Target.Create(pos);

                Destroy(orbiter);
            }

            SpawnPlanet(pos, seed);

            if (activateOrbiter)
            {
                orbiter.Activate(RandomNum.GetFloat(0f, 360, Target.RandomNumberGenerator), Target.Position.x, RandomNum.Get(0, 25, Target.RandomNumberGenerator));
                Planet planetOfTarget = Target as Planet;
                planetOfTarget.age = orbiter.Age;
            }
        }

        public abstract Type PlanetType { get; }

        public abstract void SpawnPlanet(Vector2 pos, int? seed);
    }
}
