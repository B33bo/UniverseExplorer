using UnityEngine;
using Universe.CelestialBodies.Planets;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class Aurora : CelestialBody
    {
        public const float FlareSize = 0.1f;
        public override string TypeString => "Aurora";

        public override string TravelTarget => "Lights";

        public override bool Circular => false;
        public float speed;

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

        //private AuroraColor _auroraColor;

        [InspectableVar("Color")]
        public ColorHSV AuroraColor;

        [InspectableVar("Is Rainbow")]
        public bool IsRainbow;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            RandomNumberGenerator.Next();
            Name = "Aurora " + RandomNum.GetWord(3, RandomNumberGenerator);
            Width = RandomNum.GetFloat(100, 200, RandomNumberGenerator);
            Height = RandomNum.GetFloat(7, 20f, RandomNumberGenerator);
            Flares = Mathf.CeilToInt((float)Width / FlareSize);
            Mass = 0;

            if (BodyManager.Parent is TerrestrialPlanet t && t.Name == "Earth")
            {
                if (pos.x > 0)
                    Name = "Aurora Borealis";
                else
                    Name = "Aurora Australis";
                AuroraColor = Color.magenta;
                return;
            }

            if (RandomNum.GetBool(10_000_000, RandomNumberGenerator))
                IsRainbow = true;

            AuroraColor = GetColor();
            speed = RandomNum.GetFloat(1f, RandomNumberGenerator);
        }

        private Color GetColor()
        {
            Gradient gradient = Resources.Load<GradientContainer>("Gradients/AuroraColor").gradient;
            return gradient.Evaluate((float)RandomNumberGenerator.NextDouble());
        }

        public override string GetBonusTypes()
        {
            return "Color - " + AuroraColor.ToHumanString();
        }
    }
}
