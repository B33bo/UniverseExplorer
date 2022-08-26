using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Snow
{
    public class Snowman : CelestialBody
    {
        public override string TypeString => "Snowman";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public const float Snowballs = 3;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Width = 1;
            Height = Snowballs * 3;
            Mass = 0.0008 * 3; //guess tbh
            Name = RandomNum.GetHumanName(RandomNumberGenerator);
        }
    }
}
