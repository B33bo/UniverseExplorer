using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Grass
{
    public class Grass : CelestialBody
    {
        public override string TypeString => "Grass";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Color colorOffset;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            colorOffset = Color.Lerp(Color.white, Color.green, (float)RandomNum.Get(0, 1.0, RandomNumberGenerator));
            Width = 1 * Measurement.M;
            Height = 1 * Measurement.M;
            Name = "Grass";
        }
    }
}
