using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Biomes.Grass
{
    public class TreePlant : CelestialBody
    {
        public override string TypeString => "Tree";
        public bool IsDead = false;

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

        [InspectableVar("Type")]
        public TreeColor color;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            RandomNumberGenerator.NextDouble(); // for some reason this is necessary else it all goes pink. Why? good question
            var selectedColorWeight = RandomNum.GetFloat(colorWeightTotal, RandomNumberGenerator);
            color = (TreeColor)RandomNum.GetIndexFromWeights(colorWeights, selectedColorWeight);

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
            Blossom,
            Rainbow,
        }

        public override string GetBonusTypes()
        {
            return "Color - " + color;
        }
    }
}
