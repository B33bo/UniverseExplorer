using System.Collections.Generic;
using UnityEngine;

namespace Universe.Terrain
{
    public abstract class TerrainLayer : MonoBehaviour
    {
        public abstract void Generate(Vector2Int min, Vector2Int max, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenPositions);

        public void GenerateAt(Vector2Int position, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenPositions) => Generate(position, position, ref takenPositions);

        public CelestialBodyRenderer SpawnAt(CelestialBodyRenderer block, int seed, Vector2Int position, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenBlocks)
        {
            if (takenBlocks.ContainsKey(position))
                return null;

            var newBlock = Instantiate(block);
            Vector2 posFloat = new Vector2(position.x, position.y);
            newBlock.Spawn(posFloat, seed);

            takenBlocks.Add(position, newBlock);
            return newBlock;
        }
    }
}
