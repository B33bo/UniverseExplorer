using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class GasPlanet : Planet
    {
        public const double MinScale = 5000 * Measurement.Km, MaxScale = 120000 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;
        public override string ObjectFilePos => "Objects/Planet/GasPlanet";

        public override string TypeString => "Gas Planet";

        public override string PlanetTargetScene => "GasPlanet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = GenerateName();
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }
    }
}
