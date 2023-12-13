using UnityEngine;

namespace Universe.Animals
{
    public class Butterfly : Animal
    {
        public override string DataPath => "Animals/Butterfly";

        public override string TypeString => "Butterfly";

        public Pattern leftWingPattern;  // darn Commies
        public Pattern rightWingPattern; // darn Nazis
        public float size;
        public float speed;

        public override void CreateAnimal(Animal speciesPrefab)
        {
            if (speciesPrefab is not Butterfly butterfly)
                return;

            leftWingPattern = butterfly.leftWingPattern;
            rightWingPattern = butterfly.rightWingPattern;
            leftWingPattern.id = (short)RandomNumberGenerator.Next();
            rightWingPattern.id = (short)RandomNumberGenerator.Next();
            size = butterfly.size + RandomNum.GetFloat(-.25f, .25f, RandomNumberGenerator);
            speed = butterfly.speed + RandomNum.GetFloat(-.25f, .25f, RandomNumberGenerator);

            int randomOffset = RandomNumberGenerator.Next(1_000);
            if (randomOffset < 10)
            {
                // randomize wing
                if (randomOffset < 5)
                    leftWingPattern = GenerateWing();
                else
                    rightWingPattern = GenerateWing();
            }

            Name = butterfly.leftWingPattern == butterfly.rightWingPattern ?
                leftWingPattern.ToString() + " Butterfly" : "Multi-Patterned Butterfly";

            Width = size * Measurement.cm;
            Height = Width;
        }

        public override void CreateSpecies()
        {
            leftWingPattern = GenerateWing();
            rightWingPattern = RandomNum.Get(10_000, RandomNumberGenerator) == 1 ? GenerateWing() : leftWingPattern;
            speed = RandomNum.GetFloat(1, 6f, RandomNumberGenerator);
            size = RandomNum.GetFloat(.4f, 2, RandomNumberGenerator);
        }

        private Pattern GenerateWing()
        {
            float patternChosenWeight = RandomNum.GetFloat(0, patternWeightSum, RandomNumberGenerator);

            int patternType = RandomNum.GetIndexFromWeights(WeightForPatterns, patternChosenWeight);

            return new Pattern()
            {
                id = (short)RandomNumberGenerator.Next(),

                Primary = RandomNum.GetColor(RandomNumberGenerator),
                Secondary = RandomNum.GetColor(RandomNumberGenerator),
                Tertiary = Color.black,

                patternType = (Pattern.Type)patternType,
            };
        }

        private static readonly float[] WeightForPatterns = new float[]
        {
            120,  // Solid
            20,   // Linear gradient
            1,    // Linear reflected gradient
            10,   // Circle gradient
            25,   // Diamond gradient
            20,   // Spiral gradient
            150,  // Stripes
            150,  // Spots
            20,   // Scales
            20,   // Perlin
            5,    // Worley
            75,   // Cracks
            30,   // Checkerboard
            150,  // Dalmation
            50,   // LSD
        }; private static float patternWeightSum = 846;
    }
}
