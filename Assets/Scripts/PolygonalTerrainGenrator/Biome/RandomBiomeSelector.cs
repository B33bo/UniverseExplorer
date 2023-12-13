using UnityEngine;

namespace Universe.Terrain
{
    public class RandomBiomeSelector : BiomeManager
    {
        [SerializeField]
        protected PolyTerrainLayer[] biomes;

        [SerializeField]
        protected float[] weights;

        public override PolyTerrainLayer GetBiomeAt(float x)
        {
            return biomes[RandomNum.GetIndexFromWeights(weights, GetSeedForPosition(x))];
        }
    }
}
