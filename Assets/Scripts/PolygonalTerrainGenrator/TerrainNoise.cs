using UnityEngine;

namespace Universe.Terrain
{
    [System.Serializable]
    public class TerrainNoise
    {
        public Type NoiseType = Type.PerlinNoise;
        public Vector2 Offset;
        public float Smootheness, Magnitude;
        public bool CanBeNegative;

        public float NoiseAt(Vector2 pos)
        {
            pos += Offset;
            float noise;
            switch (NoiseType)
            {
                case Type.Off:
                    return 0;
                case Type.PerlinNoise:
                    noise = Mathf.PerlinNoise(pos.x / Smootheness, pos.y / Smootheness);

                    if (CanBeNegative)
                        noise = noise * 2 - 1; // 0-1 -> -1 - 1

                    return noise * Magnitude;
                case Type.Sine:
                    noise = Mathf.Sin(pos.x / Smootheness);
                    if (!CanBeNegative)
                        noise = (noise + 1) * .5f;

                    return noise * Magnitude;
                case Type.Tan:
                    noise = (Mathf.Tan(pos.x / Smootheness) + 1) / 2;
                    noise *= Magnitude;
                    return noise;
                default:
                    return 0;
                case Type.Linear:
                    if (!CanBeNegative) return Mathf.Abs(pos.x * Magnitude);
                    return pos.x * Magnitude;
                case Type.Mountains:
                    noise = Mathf.Max(Mathf.Cos(pos.x / Smootheness), 0);
                    return noise * Magnitude;
                case Type.TriangleMountains:
                    if (Mathf.Abs(pos.x % Smootheness) < Mathf.Abs(pos.x % (2 * Smootheness)))
                        return Mathf.Abs(pos.x % Smootheness) / Smootheness * Magnitude;
                    return (-Mathf.Abs(pos.x % Smootheness) / Smootheness + 1) * Magnitude;
                case Type.Monolithe:
                    if (pos.x > -Smootheness && pos.x < Smootheness)
                        return Magnitude;
                    return 0;
            }
        }

        public enum Type
        {
            Off,
            PerlinNoise,
            Sine,
            Tan,
            Linear,
            Mountains,
            TriangleMountains,
            Monolithe,
        }
    }
}
