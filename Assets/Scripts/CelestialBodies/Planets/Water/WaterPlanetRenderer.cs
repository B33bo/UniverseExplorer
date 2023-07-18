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
        }

        public void SpawnOcean(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            (Target as WaterPlanet).IsOcean = true;
            (Target as WaterPlanet).SetOceanName();
        }
    }
}
