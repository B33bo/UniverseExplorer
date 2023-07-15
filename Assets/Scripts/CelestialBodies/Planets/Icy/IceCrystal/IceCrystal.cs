using UnityEngine;

namespace Universe.CelestialBodies.Planets.Icy
{
    public class IceCrystal : CelestialBody
    {
        public override string TypeString => "Ice Crystal";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;
        public const double Density = 0.000917;
        public float CrystalHeight;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Crystal";
            Width = Measurement.M;
            CrystalHeight = RandomNum.GetFloat(1, 5, RandomNumberGenerator);
            Height = CrystalHeight * Measurement.M;
            Mass = Height * Density;
        }
    }
}
