using System;

namespace Universe
{
    public static partial class RandomNum
    {
        public static int Get(Random random) =>
            random.Next();

        public static int Get(int min, int max, Random random) =>
            random.Next(min, max);

        // doesn't actually need to do anything, referencing the random will make it initialize
        public static void Init(Random random) { random.Next(); }

        public static double Get(double min, double max, Random random)
        {
            double randomDouble = random.NextDouble();
            return (max - min) * randomDouble + min;
        }

        public static float GetFloat(float max, Random random) =>
            (float)Get(max, random);

        public static float GetFloat(float min, float max, Random random) =>
            (float)Get(min, max, random);

        public static double Get(double max, Random random) =>
            random.NextDouble() * max;

        public static UnityEngine.Vector2 GetVector(double maxXY, Random random)
        {
            return new UnityEngine.Vector2(
                (float)Get(maxXY, random),
                (float)Get(maxXY, random));
        }

        public static UnityEngine.Vector2 GetVector(double minXY, double maxXY, Random random)
        {
            return new UnityEngine.Vector2(
                (float)Get(minXY, maxXY, random),
                (float)Get(minXY, maxXY, random));
        }

        public static string GetString(int length, Random seed)
        {
            const string StringChoices = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return GetString(length, StringChoices, seed);
        }

        public static string GetString(int length, string choices, Random random)
        {
            string s = "";

            for (int i = 0; i < length; i++)
            {
                s += choices[random.Next(0, choices.Length)];
            }

            return s;
        }

        public static int GetIndexFromWeights(float[] weights, Random random)
        {
            float total = 0;
            for (int i = 0; i < weights.Length; i++)
                total += weights[i];

            float current = GetFloat(total, random);
            int index = GetIndexFromWeights(weights, current);
            return index;
        }

        public static int GetIndexFromWeights(float[] weights, Random random, out float value)
        {
            float total = 0;
            for (int i = 0; i < weights.Length; i++)
                total += weights[i];
            value = GetFloat(total, random);
            int index = GetIndexFromWeights(weights, value);
            return index;
        }

        public static int GetIndexFromWeights(float[] weights, float number)
        {
            float sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                float next = sum + weights[i];
                if (next > number)
                    return i;
                sum = next;
            }

            return weights.Length;
        }

        public static bool GetBool(Random random) =>
            random.Next(0, 2) == 0;

        public static bool GetBool(int probability, Random random) =>
            random.Next(probability) == 1;

        public static bool GetBool(double probability, Random random) =>
            random.NextDouble() < probability;

        public static UnityEngine.Color GetColor(Random random)
        {
            return new UnityEngine.Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }

        public static UnityEngine.Color GetColor(int minHSV, int maxHSV, Random random)
        {
            float Hue = GetFloat(minHSV, maxHSV, random) / 360f;
            return UnityEngine.Color.HSVToRGB(Hue, 1, 1);
        }

        public static UnityEngine.Color GetColor(float minHSV, float maxHSV, float S, float V, Random random)
        {
            float Hue = (float)Get(minHSV, maxHSV, random) / 360f;
            S /= 255f;
            V /= 255f;
            return UnityEngine.Color.HSVToRGB(Hue, S, V);
        }

        public static double GetInfiniteDouble(double changeChance, Random random)
        {
            int direction = random.NextDouble() > .5 ? 1 : -1;
            double current = 0;
            do
            {
                current += 1;
            } while (random.NextDouble() >= changeChance);

            return current * direction;
        }

        public static double GetInfiniteDoubleAgain(double normality, Random random)
        {
            double x = random.NextDouble();
            return Math.Pow(x, normality) / (x);
        }

        public static double CurveAt(double x, double coefficient, double pwrBase)
        {
            // bm^x
            // coefficient * base ^ x

            return coefficient * Math.Pow(pwrBase, x);
        }

        public static UnityEngine.Color GetColorFromGradientAlphaWeight(UnityEngine.Gradient gradient, Random rand, int precision)
        {
            float change = 1f / precision;
            float total = 0;

            for (float f = 0; f <= 1; f += change)
                total += gradient.Evaluate(f).a;

            float numFromWeight = GetFloat(total, rand);

            for (float i = 0; i < 1; i += change)
            {
                UnityEngine.Color c = gradient.Evaluate(i);
                numFromWeight -= c.a;

                if (numFromWeight <= 0)
                {
                    c.a = 1;
                    return c;
                }
            }

            return gradient.Evaluate(1);
        }
    }
}
