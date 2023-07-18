using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class SolarFlare : CelestialBody
    {
        public override string TypeString => "Solar Flare";

        public override string TravelTarget => "";

        public override bool Circular => false;

        public float duration, speed;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Solar Flare";
            duration = RandomNum.GetFloat(1f, 10f, RandomNumberGenerator);
            speed = RandomNum.GetFloat(10f, 30f, RandomNumberGenerator);

            Width = 1;
            Height = speed * duration;
        }
    }
}
