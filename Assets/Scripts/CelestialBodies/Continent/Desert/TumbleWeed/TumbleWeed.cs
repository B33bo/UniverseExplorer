using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Desert
{
    public class TumbleWeed : CelestialBody
    {
        public override string TypeString => "Tumble Weed";

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public double rotation;

        public override void Create(Vector2 pos)
        {
            Name = "Tumble Weed";
            Position = pos;
            Radius = RandomNum.Get(0, 4, RandomNumberGenerator) * Measurement.M;

            rotation = RandomNum.Get(0, 360f, RandomNumberGenerator);
        }
    }
}
