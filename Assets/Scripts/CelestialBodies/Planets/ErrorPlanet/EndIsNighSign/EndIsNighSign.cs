using UnityEngine;

namespace Universe.CelestialBodies.Planets.Error
{
    public class EndIsNighSign : CelestialBody
    {
        public const float TheEnd = 16_777_216; // this is the limit because in floating point, you can no longer increment by 1. Needs to be > 1

        public override string TypeString => "Sign";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "The End Is Nigh";
            Width = 4 * Measurement.M;
            Height = 4 * Measurement.M;
        }

        public override string GetBonusTypes()
        {
            return "Positive - " + (Position.x > 0);
        }
    }
}
