using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class MoltenPlanet : Planet
    {
        public const double MinScale = 100 * Measurement.Km, MaxScale = 6371 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;
        public override string ObjectFilePos => "Objects/MoltenPlanet";

        public override string TypeString => "Molten Planet";

        public override bool Circular => true;

        public override string PlanetTargetScene => "MoltenPlanet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }
    }
}
