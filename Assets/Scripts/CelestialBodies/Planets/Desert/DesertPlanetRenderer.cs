using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets.Desert
{
    public class DesertPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(DesertPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = (float)GetFairSize(Target.Radius, DesertPlanet.minRadius, DesertPlanet.maxRadius) * 2 * Vector2.one;
        }
    }
}
