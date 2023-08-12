using System;
using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class ToxicPlanet : Planet
    {
        public override string ObjectFilePos => "Objects/Planet/Toxic";

        public override string PlanetTargetScene => "ToxicPlanet";

        public override string TypeString => "Toxic Planet";

        public const double MinScale = 1000, MaxScale = 12000;

        [InspectableVar("Color")]
        public Color ToxicColor;

        [InspectableVar("Type")]
        public ColorType Type;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = GenerateName();
            Type = (ColorType)RandomNum.GetIndexFromWeights(WeightOfColor, RandomNum.GetFloat(0, weightOfColorSum, RandomNumberGenerator));
            ToxicColor = ColorOfType[(int)Type];
            ToxicColor.r += RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
            ToxicColor.g += RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
            ToxicColor.b += RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);

            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = RandomNum.Get(3e22 * Measurement.Kg, 4e24 * Measurement.Kg, RandomNumberGenerator);
        }

        public enum ColorType
        {
            Purple,
            Green,
            Black,
            Yellow,
            Cyan,
            White
        }

        private static readonly float[] WeightOfColor = new float[]
        {
            100,
            50,
            2,
            1,
            1,
            .5f,
        }; private const float weightOfColorSum = 151;

        private static readonly Color[] ColorOfType = new Color[]
        {
            new Color(178 / 255f, 0, 1),
            new Color(58 / 255f, 193 / 255f, 0),
            new Color(0, 0, 0),
            new Color(1, 1, .5f),
            new Color(129 / 255f, 1, 247 / 255f),
            new Color(.9f, .9f, .9f),
        };
    }
}
