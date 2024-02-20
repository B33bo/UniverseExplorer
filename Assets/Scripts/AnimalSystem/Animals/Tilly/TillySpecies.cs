using UnityEngine;

namespace Universe.Animals
{
    public class TillySpecies : AnimalSpecies
    {
        public override string AnimalName => "Tilly";

        public Tail[] Tails;

        public override void Create(System.Random rand)
        {
            Pattern pattern = GetPattern(rand);
            speciesName = RandomNum.GetWord(3, rand);
            Legs = new Leg[2];
            float leglength = RandomNum.GetFloat(.75f, 1.25f, rand);

            BodyPart.InitBodyParts(Legs, (i, leg) =>
            {
                leg.length = leglength;
                leg.pattern = pattern;
            });

            Eyes = new Eye[1];
            BodyPart.InitBodyParts(Eyes, (i, eye) => 
            { 
                eye.Strength = 10;
                eye.Radius = .13f;
            });

            Tails = new Tail[1];
            float tailLength = RandomNum.GetFloat(.5f, 1.5f, rand);

            BodyPart.InitBodyParts(Tails, (i, tail) =>
            {
                tail.length = tailLength;
                tail.wagSpeed = 1;
                tail.pattern = pattern;
            });

            torso = new Torso
            {
                pattern = pattern
            };

            walkSpeed = RandomNum.GetFloat(.4f, 4f, rand);
        }

        public Pattern GetPattern(System.Random rand)
        {
            float[] weights = new float[patternWeights.Length];

            for (int i = 0; i < weights.Length; i++)
                weights[i] = patternWeights[i].weight;

            Pattern.Type type = patternWeights[RandomNum.GetIndexFromWeights(weights, rand)].pattern;

            Color primary = RandomNum.GetColor(rand);
            Color secondary = RandomNum.GetColor(rand);
            Color tertiary = RandomNum.GetColor(rand);

            return new Pattern()
            {
                Primary = primary,
                Secondary = secondary,
                Tertiary = tertiary,
                patternType = type,
            };
        }

        private static readonly (Pattern.Type pattern, float weight)[] patternWeights = new (Pattern.Type, float)[]
        {
            ( Pattern.Type.Solid, 0 ),
            ( Pattern.Type.LinearGradient, 75 ),
            ( Pattern.Type.LinearGradient, 75 ),
            ( Pattern.Type.CircleGradient, 50 ),
            ( Pattern.Type.DiamondGradient, 30 ),
            ( Pattern.Type.SpiralGradient, 60 ),
            ( Pattern.Type.Stripes, 75 ),
            ( Pattern.Type.Spots, 75 ),
            ( Pattern.Type.Perlin, 40 ),
            ( Pattern.Type.Worley, 50 ),
            ( Pattern.Type.Cracks, 65 ),
            ( Pattern.Type.Checkerboard, 10 ),
            ( Pattern.Type.Dalmation, 120 ),
            ( Pattern.Type.LSD, 5 ),
        };
    }
}
