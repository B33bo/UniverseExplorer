using UnityEngine;

namespace Universe.CelestialBodies
{
    public class BlackHole : CelestialBody
    {
        public const double MinScale = 24 * Measurement.Km, MaxScale = 1000 * Measurement.Km;
        public const double MinMass = 2e30 * Measurement.Kg, MaxMass = 3e31 * Measurement.Kg;

        public float RotateSpeed;

        public override void Create(Vector2 position)
        {
            Position = position;

            Name = RandomNum.GetString(1, RandomNumberGenerator) + RandomNum.Get(10, 100, RandomNumberGenerator);

            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);

            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        public override string TypeString => "Black Hole";

        public override string TravelTarget => "BlackHole";

        public override bool Circular => true;
    }
}
