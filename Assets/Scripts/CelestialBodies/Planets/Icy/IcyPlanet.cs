using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class IcyPlanet : Planet
    {
        public const double MinScale = 100 * Measurement.Km, MaxScale = 6371 * Measurement.Km;
        public const double MinMass = 1.5e22 * Measurement.Kg, MaxMass = 2e24 * Measurement.Kg;

        public override string TypeString => "Icy Planet";

        public override bool Circular => true;

        public override string ObjectFilePos => "Objects/Planet/IcyPlanet";

        public override string PlanetTargetScene => "IcyPlanet";

        public override void Create(Vector2 pos)
        {
            Position = pos;

            if (Seed == Star.Pluto)
            {
                Name = "Pluto";
                Radius = 1188.3 * Measurement.Km;
                Mass = 1.309e22 * Measurement.Kg;
            }
            else
            {
                Name = GenerateName();
                Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
                Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
            }
        }
    }
}
