using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Grass
{
    public class TreePlant : CelestialBody
    {
        public override string TypeString => "Tree";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public static readonly float[] colorWeights = new float[]
        {
            100,//green
            100, //dead
            25, //red
            50, //orange
            30, //pink
            1,  //rainbow
        };
        private const float colorWeightTotal = 306;

        public TreeColor color;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            var selectedColorWeight = RandomNum.GetFloat(colorWeightTotal, RandomNumberGenerator);
            color = (TreeColor)RandomNum.GetIndexFromWeight(colorWeights, selectedColorWeight);

            Name = "Tree";

            Width = 1 * Measurement.M;
            Height = 2 * Measurement.M;
        }

        public enum TreeColor
        {
            Green,
            Dead,
            Red,
            Orange,
            Pink,
            Rainbow,
        }
    }
}
