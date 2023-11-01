using UnityEngine;

namespace Universe
{
    public class Quark : CelestialBody
    {
        public override string TypeString => Type;

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public string Type = "Quark", Symbol = "?", Charge = "0", Spin = "0";

        public override void Create(Vector2 pos)
        {
            Position = pos;
        }

        public override string GetBonusTypes()
        {
            return $"Type - {Type}\nCharge - {Charge}\nSpin - {Spin}";
        }
    }
}
