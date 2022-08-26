using UnityEngine;

namespace Universe.CelestialBodies
{
    public class SupermassiveBlackHole : BlackHole
    {
        public new const double MinScale = 15e7 * Measurement.Km, MaxScale = 1.9e15 * Measurement.Km;
        public new const double MinMass = 8.5e35 * Measurement.Kg, MaxMass = 1.3e41 * Measurement.Kg;

        public override void Create(Vector2 position)
        {
            Position = position;

            Name = RandomNum.GetString(3, RandomNumberGenerator) + " " + RandomNum.Get(10, 100, RandomNumberGenerator);
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        public override string TypeString => "Supermassive black hole";

        public override string TravelTarget => "BlackHole";

        public override bool Circular => true;
    }
}
