using System.Collections.Generic;
using UnityEngine;

namespace Universe.Terrain
{
    public class MultipleBlocks : TerrainLayer
    {
        [SerializeField]
        private CelestialBodyRenderer[] blocks;

        [SerializeField]
        private float[] weights;
        private float maxWeight;

        private void Start()
        {
            for (int i = 0; i < weights.Length; i++)
                maxWeight += weights[i];
        }

        public override void Generate(Vector2Int bottomLeft, Vector2Int topRight, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenPositions)
        {
            for (int x = bottomLeft.x; x <= topRight.x; x++)
            {
                for (int y = bottomLeft.y; y <= topRight.y; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    int seed = ((Vector2)position).HashPos(BodyManager.GetSeed());
                    SpawnAt(blocks[GetIndex(seed)], seed, position, ref takenPositions);
                }
            }
        }

        private int GetIndex(int seed)
        {
            System.Random rand = new System.Random(seed);
            return RandomNum.GetIndexFromWeights(weights, RandomNum.GetFloat(maxWeight, rand));
        }
    }
}
