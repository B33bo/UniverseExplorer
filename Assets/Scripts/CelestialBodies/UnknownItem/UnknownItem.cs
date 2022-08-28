using UnityEngine;

namespace Universe.CelestialBodies
{
    public class UnknownItem : CelestialBody
    {
        public override string TypeString => "Placeholder item";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Placeholder";
        }
    }
}
