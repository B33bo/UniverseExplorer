using System;
using System.Collections;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public abstract class PlanetRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            if (BodyManager.Parent is Planet && !(BodyManager.Parent is Moon))
            {
                Target = BodyManager.Parent;
                Target.SetSeed(Target.Seed);
                Target.Create(pos);

                SpawnPlanet(pos, seed);
            }
            else
            {
                Target = (Planet)Activator.CreateInstance(PlanetType);
                if (seed.HasValue)
                    Target.SetSeed(seed.Value);
                Target.Create(pos);

                SpawnPlanet(pos, seed);

                if (!(BodyManager.Parent is Moon))
                    StartCoroutine(SpawnMoons());
            }
        }

        private IEnumerator SpawnMoons()
        {
            yield return new WaitForEndOfFrame();
            (Target as Planet).SpawnMoons(transform);
        }

        public override void OnUpdate()
        {
            Debug.DrawLine(transform.position, transform.parent.position);
            transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);
        }

        public abstract Type PlanetType { get; }

        public abstract void SpawnPlanet(Vector2 pos, int? seed);
    }
}
