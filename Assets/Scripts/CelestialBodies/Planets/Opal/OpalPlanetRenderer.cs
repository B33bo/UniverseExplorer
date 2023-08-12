using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class OpalPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(OpalPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Target = new OpalPlanet();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);

            Scale = (float)GetFairSize(Target.Width, OpalPlanet.MinScale, OpalPlanet.MaxScale) * Vector2.one;
        }
    }
}
