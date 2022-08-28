using UnityEngine;

namespace Universe.CelestialBodies.Planets.Error
{
    public class EndIsNighSign : CelestialBody
    {
        //fun fact, this is he number I got while testing what position my game was crashing at
        public const float TheEnd = 8_388_532;

        public override string TypeString => "Sign";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "The End Is Nigh";
            Width = 4;
            Height = 4;
        }

        public override string GetBonusTypes()
        {
            return "Positive - " + (Position.x > 0);
        }
    }
}
