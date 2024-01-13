using UnityEngine;

namespace Universe.CelestialBodies.Planets.Icy
{
    public class ChristmasTree : CelestialBody
    {
        public override string TypeString => "Christmas Tree";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Vector2[] BranchWidths;
        public const float Scale = 1;
        public const float Falloff = .2f;

        public Color baubleColor1, baubleColor2;
        public Color starColor;
        public Color tinselColor;

        public int starPoints;

        public override void Create(Vector2 pos)
        {
            Name = "Christmas Tree";
            Position = pos;

            BranchWidths = new Vector2[8];

            BranchWidths[0] = new Vector2(-BranchWidths.Length * .2f, 0);

            for (int i = 1; i < BranchWidths.Length - 2; i += 2)
            {
                float multiplier = BranchWidths.Length - i;
                multiplier *= Falloff; // multiplier multiplier

                BranchWidths[i] = new Vector2(-multiplier * .5f, (i + 1) * Scale * .5f);
                BranchWidths[i + 1] = new Vector2(-multiplier, (i + 1) * Scale * .5f);
            }

            BranchWidths[^1] = new Vector2(0, BranchWidths.Length * Scale * .5f);

            RandomNumberGenerator.Next();
            RandomNumberGenerator.Next();
            RandomNumberGenerator.Next();
            Gradient colors = Resources.Load<GradientContainer>("Gradients/ChristmasTree");
            Gradient starColors = Resources.Load<GradientContainer>("Gradients/ChristmasTreeStar");

            baubleColor1 = colors.Evaluate((float)RandomNumberGenerator.NextDouble());
            baubleColor2 = colors.Evaluate((float)RandomNumberGenerator.NextDouble());

            starColor = starColors.Evaluate((float)RandomNumberGenerator.NextDouble());

            if (RandomNum.GetBool(RandomNumberGenerator))
                tinselColor = starColors.Evaluate((float)RandomNumberGenerator.NextDouble());
            else
                tinselColor = Color.HSVToRGB((float)RandomNumberGenerator.NextDouble(), 1, 1);

            starPoints = RandomNum.Get(3, 8, RandomNumberGenerator);
        }
    }
}
