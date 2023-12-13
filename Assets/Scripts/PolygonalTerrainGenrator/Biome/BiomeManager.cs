using UnityEngine;

namespace Universe.Terrain
{
    public abstract class BiomeManager : MonoBehaviour
    {
        [SerializeField]
        protected float biomeWidth = 100;

        public float biomeHeight = 10;

        public abstract PolyTerrainLayer GetBiomeAt(float x);

        protected System.Random GetSeedForPosition(float x)
        {
            x = GetBeforeBiome(x);
            int seed = new Vector2(x, x * .1f).HashPos(BodyManager.GetSeed());
            return new(seed);
        }

        protected float GetBeforeBiome(float x)
        {
            return Mathf.Floor(x / biomeWidth) * biomeWidth;
        }
    }
}
