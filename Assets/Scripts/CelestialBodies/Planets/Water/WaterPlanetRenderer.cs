using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class WaterPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(WaterPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            transform.localScale = GetFairSize((float)Target.Width, (float)TerrestrialPlanet.MinScale, (float)TerrestrialPlanet.MaxScale) * Vector2.one;
        }
    }
}
