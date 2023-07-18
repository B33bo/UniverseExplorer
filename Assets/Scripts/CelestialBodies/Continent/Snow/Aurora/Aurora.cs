using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class Aurora : CelestialBody
    {
        public const float FlareSize = 0.1f;
        public override string TypeString => "Aurora";

        public override string TravelTarget => "Lights";

        public override bool Circular => false;

        public static readonly float[] WeightChart = new float[]
        {
            100, //Green
            50, //Pink
            10, //Blue
            1, //Red
            0.1f, // Rainbow
        };

        public static readonly ColorHSV[] colorsOfAurora = new ColorHSV[]
        {
            new ColorHSV(136 / 360f, 1, 1), //Green
            new ColorHSV(310 / 360f, .81f, 1), //Pink
            new ColorHSV(221 / 360f, 1, 1), //Blue
            new ColorHSV(0, .93f, 1), //Red
            new ColorHSV(0, 0, 0), //Rainbow
        };

        const float sumOfWeightChart = 161.1f;

        public int Flares;

        private Color _color;
        private AuroraColor _auroraColor;

        [InspectableVar("Color")]
        public ColorHSV Color { get => _color; set => _color = value; }

        [InspectableVar("Color Type")]
        public AuroraColor AuroraCol
        {
            get => _auroraColor; set
            {
                _auroraColor = value;
                _color = colorsOfAurora[(int)value];
            }
        }

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Aurora " + RandomNum.GetPlanetName(RandomNumberGenerator);
            Width = RandomNum.GetFloat(8, 56, RandomNumberGenerator);
            Height = 5;
            Flares = Mathf.CeilToInt((float)Width / FlareSize);
            Mass = 0;

            float randomWeight = RandomNum.GetFloat(0, sumOfWeightChart, RandomNumberGenerator);
            //fine because the weights correspond to the index of the enum (green = 0)
            AuroraCol = (AuroraColor)RandomNum.GetIndexFromWeight(WeightChart, randomWeight);
        }

        public override string GetBonusTypes()
        {
            return "Color - " + AuroraCol;
        }

        public enum AuroraColor
        {
            Green,
            Pink,
            Blue,
            Red,
            Rainbow,
        }
    }
}
