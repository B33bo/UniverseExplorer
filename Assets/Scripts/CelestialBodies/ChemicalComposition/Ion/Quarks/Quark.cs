using UnityEngine;

namespace Universe
{
    public class Quark : CelestialBody
    {
        public override string TypeString => Type;

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public string Type = "Quark", Symbol = "?";

        public override void Create(Vector2 pos)
        {
            Position = pos;
        }
    }
}
