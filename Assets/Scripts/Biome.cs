using UnityEngine;

namespace Universe
{
    [CreateAssetMenu(fileName = "biome", menuName = "biome")]
    public class Biome : ScriptableObject
    {
        public CelestialBodyRenderer[] objects;
        public float[] weights;

        public float MaxHeight = 2;

        public Color groundColor;
        public bool colorGround = true;
        public SpriteRenderer groundPrefab;

        public Biome[] adjacentBiomes;
        public float[] biomeWeights;
    }
}
