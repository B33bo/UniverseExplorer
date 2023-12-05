using UnityEngine;

namespace Universe.CelestialBodies.Planets.Molten
{
    public class Volcano : CelestialBody
    {
        public override string TypeString => "Volcano";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public const double MinMass = 200_000_000 * Measurement.Kg, MaxMass = 600_000_000 * Measurement.Kg;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Width = pos.y * 2;
            Height = pos.y * 2;
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
            Name = "Mount " + RandomNum.GetWord(4, RandomNumberGenerator);
        }
    }
}
