using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class MoltenPlanet : Planet
    {
        public const double MinScale = 100 * Measurement.Km, MaxScale = 6371 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;
        public override string ObjectFilePos => "Objects/Planet/MoltenPlanet";

        public override string TypeString => "Molten Planet";

        public override bool Circular => true;

        public override string PlanetTargetScene => "MoltenPlanet";

        public override void Create(Vector2 pos)
        {
            Position = pos;

            if (Seed == Star.Venus)
            {
                Name = "Venus";
                Radius = 6051 * Measurement.Km;
                Mass = 4.867e27 * Measurement.Kg;
                return;
            }

            Name = GenerateName();
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }
    }
}
