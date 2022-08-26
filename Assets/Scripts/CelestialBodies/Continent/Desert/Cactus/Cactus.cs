using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Desert
{
    public class Cactus : CelestialBody
    {
        public override string TypeString => "Cactus";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Cactus";
            Width = RandomNum.Get(.5, 2, RandomNumberGenerator) * Measurement.M;
            Height = RandomNum.Get(.5, 2, RandomNumberGenerator) * Measurement.M;

            Mass = 2;
        }
    }
}
