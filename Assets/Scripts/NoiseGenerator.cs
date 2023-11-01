using UnityEngine;

namespace Universe
{
    public static class NoiseGenerator
    {
        public static Texture2D WorleyNoise(int seed, int pointCount, int size, Color a, Color b)
        {
            Texture2D noise = new Texture2D(size, size);
            Vector2[] points = new Vector2[pointCount];

            System.Random rand = new System.Random(seed);
            for (int i = 0; i < points.Length; i++)
            {
                // this uses a percentage-based approcach.
                // this is because if someone is playing on low graphics, their worley noise looks the same as
                // that of someone who is using high graphics.
                float x = RandomNum.GetFloat(1f, rand) * size;
                float y = RandomNum.GetFloat(1f, rand) * size;
                points[i] = new Vector2(x, y);
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float distance = GetMinDistance(x, y);
                    distance /= size / 2; // average of width and height / 2
                    noise.SetPixel(x, y, Color.Lerp(a, b, distance));
                }
            }

            noise.Apply();
            return noise;

            float GetMinDistance(int x, int y)
            {
                float minDistance = float.PositiveInfinity;
                for (int i = 0; i < points.Length; i++)
                {
                    float distance = (points[i] - new Vector2(x, y)).sqrMagnitude;
                    if (distance < minDistance)
                        minDistance = distance;
                }
                return Mathf.Sqrt(minDistance);
            }
        }
    }
}
