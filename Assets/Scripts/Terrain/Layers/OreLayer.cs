using System.Collections.Generic;
using UnityEngine;
using Universe.Blocks;

namespace Universe.Terrain
{
    public class OreLayer : TerrainLayer
    {
        [SerializeField]
        private OreRarity[] ores;

        [SerializeField]
        private Sprite rockSprite;

        [SerializeField]
        private CelestialBodyRenderer rock;

        [SerializeField]
        private OreBlockRenderer oreBlock;

        [SerializeField]
        private int OreSize = 6;

        private Vector2Int min, max;

        private int ToLowestCellSize(int pos)
        {
            int i;
            for (i = 0; (pos + i) % OreSize != 0; i--) ;
            return pos + i;
        }

        public override void Generate(Vector2Int min, Vector2Int max, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenPositions)
        {
            this.min = min;
            this.max = max;
            min.x = ToLowestCellSize(min.x);
            min.y = ToLowestCellSize(min.y);
            // miny % 6 not same ?

            for (int x = min.x; x <= max.x; x += OreSize)
            {
                for (int y = min.y; y <= max.y; y += OreSize)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    int seed = ((Vector2)position).HashPos(BodyManager.GetSeed());
                    Vector2Int maxPos = position + new Vector2Int(OreSize, OreSize);
                    SpawnOreVein(position, maxPos, seed, max.y, ref takenPositions);
                }
            }
        }

        public CelestialBodyRenderer SpawnAtOre(OreBlockRenderer block, OreType oreType, Vector2Int position, int seed, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenBlocks)
        {
            if (takenBlocks.ContainsKey(position))
                return null;
            if (position.x < min.x || position.x > max.x)
                return null;
            if (position.y < min.y || position.y > max.y)
                return null;

            Vector2 posFloat = new Vector2(position.x, position.y);
            var newBlock = Instantiate(block);
            newBlock.Spawn(posFloat, seed, oreType, rockSprite);
            takenBlocks.Add(position, newBlock);
            return newBlock;
        }

        public override CelestialBodyRenderer SpawnAt(CelestialBodyRenderer block, int seed, Vector2Int position, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenBlocks)
        {
            if (position.x < min.x || position.x > max.x)
                return null;
            if (position.y < min.y || position.y > max.y)
                return null;
            return base.SpawnAt(block, seed, position, ref takenBlocks);
        }

        private OreType? GetOreType(System.Random rand, float depth)
        {
            float weightSum = 0;
            float[] weights = new float[ores.Length];

            for (int i = 0; i < ores.Length; i++)
            {
                if (ores[i].MaxDepth < depth)
                {
                    weights[i] = 0;
                    continue;
                }

                float weight = ores[i].Weight * ores[i].WeightMultiplier.Evaluate(depth / ores[i].MaxDepth).r;
                weightSum += weight;
                weights[i] = weight;
            }

            if (weightSum == 0)
                return null;

            float chosenWeightValue = RandomNum.GetFloat(weightSum, rand);
            int index = RandomNum.GetIndexFromWeights(weights, chosenWeightValue);
            var oreType = OreType.Ores[(int)ores[index].OreType];
            return oreType;
        }

        private void SpawnOreVein(Vector2Int min, Vector2Int max, int seed, float maxAllowedObject, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenBlocks)
        {
            // max allowed object stops from leeking above the current layer
            Vector2Int start = (min + max) / 2;
            System.Random rand = new System.Random(seed);

            var nullableOreType = GetOreType(rand, -start.y);

            if (!nullableOreType.HasValue)
                return;

            OreType oreType = nullableOreType.Value;
            bool doOres = oreType.Color != Color.clear;

            List<Vector2Int> ores = new List<Vector2Int>();

            if (doOres)
            {
                ores.Add(start); // avoids "none" ores
                Branch(start, min, max);
            }

            for (int x = min.x; x < max.x; x++)
            {
                for (int y = min.y; y < max.y; y++)
                {
                    Vector2Int newPos = new Vector2Int(x, y);
                    if (newPos.y > maxAllowedObject)
                        continue;
                    if (ores.Contains(newPos))
                        SpawnAtOre(oreBlock, oreType, newPos, 0, ref takenBlocks);
                    else 
                        SpawnAt(rock, 0, newPos, ref takenBlocks);
                }
            }

            void Branch(Vector2Int position, Vector2Int min, Vector2Int max)
            {
                List<Vector2Int> possibilities = new List<Vector2Int>()
                {
                    position + new Vector2Int(-1, 1),
                    position + new Vector2Int(0, 1),
                    position + new Vector2Int(1, 1),
                    position + new Vector2Int(-1, 0),
                    position + new Vector2Int(1, 0),
                    position + new Vector2Int(-1, 1),
                    position + new Vector2Int(0, -1),
                    position + new Vector2Int(1, -1),
                };

                int choices = rand.Next(0, 3);
                for (int i = 0; i < choices; i++)
                {
                    int index = rand.Next(0, possibilities.Count);
                    Vector2Int newPos = possibilities[index];
                    possibilities.RemoveAt(index);

                    if (ores.Contains(newPos))
                        continue;
                    if (!IsInBounds(newPos, min, max))
                        continue;

                    ores.Add(newPos);
                    Branch(newPos, min, max);
                }
            }

            bool IsInBounds(Vector2Int pos, Vector2Int min, Vector2Int max) =>
                pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y;
        }

        [System.Serializable]
        private class OreRarity
        {
            public OreType.EnumNames OreType;
            public float Weight;
            public Gradient WeightMultiplier;
            public float MaxDepth;
        }
    }
}
