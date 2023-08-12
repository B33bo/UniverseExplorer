using UnityEngine;

namespace Universe.CelestialBodies.Planets.Toxic
{
    public class ToxicGas : CelestialBody
    {
        public override string TypeString => "Toxic Gas";

        public override string TravelTarget => "";

        public override bool Circular => false;

        public float GasWidth;
        public float Speed;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            GasWidth = RandomNum.GetFloat(0, 10, RandomNumberGenerator);
            Speed = RandomNum.GetFloat(2, 10, RandomNumberGenerator);
            Width = Measurement.M * GasWidth;
            Height = Measurement.M;
        }
    }
}
