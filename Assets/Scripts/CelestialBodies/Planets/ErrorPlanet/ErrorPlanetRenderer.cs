using System;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class ErrorPlanetRenderer : PlanetRenderer
    {
        public override Type PlanetType => typeof(ErrorPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {

        }
    }
}
