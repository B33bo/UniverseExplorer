using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class WaterPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(WaterPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Width, (float)TerrestrialPlanet.MinScale, (float)TerrestrialPlanet.MaxScale) * Vector2.one;
            sprite.color = (Target as Planet).waterColor;
        }
    }
}
