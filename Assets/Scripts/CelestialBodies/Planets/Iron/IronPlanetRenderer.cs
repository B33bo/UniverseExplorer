using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class IronPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(IronPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Target = new IronPlanet();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
            Scale = GetFairSize((float)Target.Radius, (float)IronPlanet.MinScale, (float)IronPlanet.MaxScale) * Vector2.one;
        }
    }
}
