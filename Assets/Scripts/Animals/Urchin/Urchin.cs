using UnityEngine;
using Universe.Animals;

namespace Universe.AnimalsOld
{
    public class Urchin : AnimalOld
    {
        public override string TypeString => "Urchin";

        public override string DataPath => "Animals/Urchin";

        public Pattern pattern;
        private bool isInverted;
        public float size;

        public override void CreateAnimal(AnimalOld speciesPrefab)
        {
            if (!(speciesPrefab is Urchin urchin))
                return;

            Name = urchin.Name;
            pattern = urchin.pattern;
            pattern.id = (short)RandomNumberGenerator.Next();
            size = urchin.size + RandomNum.GetFloat(-.5f, .5f, RandomNumberGenerator);

            if (size < 0)
                size = .1f;

            Radius = size * Measurement.cm;

            if (RandomNum.Get(0, 200, RandomNumberGenerator) == 1)
            {
                isInverted = true;
                pattern.Primary = new Color(1 - pattern.Primary.r, 1 - pattern.Primary.g, 1 - pattern.Primary.b);
            }
        }

        private Color GetColor()
        {
            Color c = RandomNum.GetColor(RandomNumberGenerator) / 3;
            c.a = 1;

            if (isInverted)
                return new Color(1 - c.r, 1 - c.g, 1 - c.b);
            return c;
        }

        public override void CreateSpecies()
        {
            isInverted = RandomNum.Get(0, 200, RandomNumberGenerator) == 6;

            int patternType = RandomNum.GetIndexFromWeights(WeightForPatterns, RandomNum.GetFloat(0, patternWeightSum, RandomNumberGenerator));

            pattern = new Pattern
            {
                rotation = RandomNum.GetFloat(360f, RandomNumberGenerator),

                Primary = GetColor(),
                Secondary = GetColor(),
                Tertiary = GetColor(),

                patternType = (Pattern.Type)patternType,
            };

            size = RandomNum.GetFloat(.2f, 3f, RandomNumberGenerator);
            Name = pattern.ToString() + " Urchin";
        }

        private static readonly float[] WeightForPatterns = new float[]
        {
            300, // Solid
            1,   // Linear gradient
            1,   // Linear reflected gradient
            100, // Circle gradient
            25,  // Diamond gradient
            45,  // Spiral gradient
            15,  // Stripes
            100, // Spots
            5,   // Scales
            40,  // Perlin
            40,  // Worley
            75,  // Cracks
            2,   // Checkerboard
            100, // Dalmation
            50,  // LSD
        }; private const float patternWeightSum = 899;
    }
}
