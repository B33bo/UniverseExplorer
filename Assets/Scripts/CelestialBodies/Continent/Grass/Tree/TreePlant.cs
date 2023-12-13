using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Biomes.Grass
{
    public class TreePlant : CelestialBody
    {
        public override string TypeString => "Tree";
        public bool IsDead = false;
        private bool setType = false;

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public static readonly float[] colorWeights = new float[]
        {
            100, // green
            100, // dead
            25,  // red
            50,  // orange
            30,  // pink
            1,   // rainbow
            30,  // custom
        };
        private const float colorWeightTotal = 336;

        [InspectableVar("Type")]
        public TreeColor colorType;

        [InspectableVar("Color")]
        public Color color;

        private static readonly Color[] ColorValuesForLeaves = new Color[]
        {
            new Color(0, .8f, 0), // Green
            new Color(0, 0, 0, 0), // Dead / transparent
            new Color(1, 0, 0), // Red
            new Color(1, .5f, 0), // Orange
            new Color(1, .5f, 1), // Pink
            new Color(1, 1, 1), // Rainbow
            new Color(0, 0, 0), // Rand
        };

        public void SetType(System.Random rand)
        {
            if (setType)
                return;

            setType = true;
            rand.Next(); // initialise it or smth

            float selectedColorWeight = RandomNum.GetFloat(colorWeightTotal, rand);
            SetTreeColorToType((TreeColor)RandomNum.GetIndexFromWeights(colorWeights, selectedColorWeight), rand);
        }

        public void SetTreeColorToType(TreeColor colorType, System.Random rand)
        {
            this.colorType = colorType;

            if (colorType == TreeColor.Random)
                color = RandomNum.GetColor(rand);
            else
                color = ColorValuesForLeaves[(int)colorType];
        }

        public override void Create(Vector2 pos)
        {
            Position = pos;

            SetType(RandomNumberGenerator);

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
            Random,
        }

        public override string GetBonusTypes()
        {
            if (IsDead)
                return "";
            return "Color - " + (colorType == TreeColor.Random ? color.ToHumanString() : colorType.ToString());
        }
    }
}
