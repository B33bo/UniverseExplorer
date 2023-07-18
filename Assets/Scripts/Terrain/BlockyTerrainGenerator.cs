using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Universe
{
    public class BlockyTerrainGenerator : Spawner
    {
        public float Steepness;
        public float Wideness = 1;

        [SerializeField]
        private CelestialBodyRenderer topBlock;

        [SerializeField]
        private LayerData[] layers;

        private Dictionary<Vector2Int, CelestialBodyRenderer> blocksTaken;
        private Rect cameraRect;
        private Dictionary<int, float> perlinCache = new Dictionary<int, float>();
        private Vector2 lastReload;
        private int totalDepth;

        public override void OnStart()
        {
            blocksTaken = new Dictionary<Vector2Int, CelestialBodyRenderer>();
            base.OnStart();

            for (int i = 0; i < layers.Length; i++)
                totalDepth += layers[i].height;
        }

        public override void ReloadCells(Rect cameraRect)
        {
            if ((cameraRect.position - lastReload).sqrMagnitude < .8f)
                return;
            this.cameraRect = cameraRect;
            lastReload = cameraRect.position;
            RemoveOldCells(null);
            GenerateNewCells(null);
        }

        public override void GenerateNewCells(List<Vector2> cellsOnScreen)
        {
            int depth = 0;

            int xMin = Mathf.FloorToInt(cameraRect.xMin) - 1;
            int xMax = Mathf.CeilToInt(cameraRect.xMax) + 1;

            if (cameraRect.yMax > 0)
                GenerateTopLayer(xMin, xMax);

            for (int i = 0; i < layers.Length; i++)
            {
                if (depth > -cameraRect.yMin)
                    break;

                float minY = Mathf.Max(cameraRect.yMin, -depth - layers[i].height);
                float maxY = Mathf.Min(cameraRect.yMax, -depth);
                Vector2Int min = new Vector2Int(xMin, (int)minY - 1);
                Vector2Int max = new Vector2Int(xMax, (int)maxY);

                if (!IsInBounds(min, 2))
                    continue;

                depth += layers[i].height;
                layers[i].layer.Generate(min, max, ref blocksTaken);
            }

            if (depth > -cameraRect.yMin)
                return;

            Vector2Int finishMin = new Vector2Int(xMin, (int)cameraRect.yMin - 1);
            Vector2Int finishMax = new Vector2Int(xMax, (int)Mathf.Min(-totalDepth + 3, cameraRect.yMax + 3));
            layers[layers.Length - 1].layer.Generate(finishMin, finishMax, ref blocksTaken);
        }

        private void GenerateTopLayer(int xMin, int xMax)
        {
            if (perlinCache.Count > 5000)
                perlinCache = new Dictionary<int, float>();

            if (topBlock == null)
                return;
            int seed = BodyManager.GetSeed();
            for (int i = xMin; i < xMax; i++)
            {
                float perlinVal;
                if (perlinCache.ContainsKey(i))
                    perlinVal = perlinCache[i];
                else
                {
                    perlinVal = Mathf.PerlinNoise(i * Wideness, seed);
                    perlinCache.Add(i, perlinVal);
                }

                float heightValue = perlinVal * Steepness + 1;
                Vector2Int position = new Vector2Int(i, (int)heightValue);

                Vector2Int spawnAtPos = new Vector2Int(position.x, position.y + 1);
                if (!blocksTaken.ContainsKey(spawnAtPos))
                    SpawnAtPos(spawnAtPos);

                if (!IsInBounds(position, 2))
                    continue;

                if (blocksTaken.ContainsKey(position))
                    continue;

                var block = Instantiate(topBlock);
                block.Spawn(position, ((Vector2)position).HashPos(BodyManager.GetSeed()));
                blocksTaken.Add(position, block);

                position.y--;
                layers[0].layer.Generate(new Vector2Int(position.x, 1), position, ref blocksTaken);
            }
        }

        public CelestialBodyRenderer SpawnAtPos(Vector2 position)
        {
            if (objects.Length == 0)
                return null;

            int seed = BodyManager.GetSeed();
            int positionSeed = position.HashPos(seed);

            var rand = new System.Random(positionSeed);

            var target = objects[GetSeededIndex(weights, rand)];

            if (target is null)
                return null;

            var newObject = SpawnAt(target, position, positionSeed);

            return newObject;
        }

        public override CelestialBodyRenderer SpawnAt(CelestialBodyRenderer prefab, Vector2 position, int? seed)
        {
            if (prefab is null)
                return null;
            CelestialBodyRenderer newObject = Instantiate(prefab);
            newObject.Spawn(new Vector2(position.x, position.y - .5f), seed);
            blocksTaken.Add(new Vector2Int((int)position.x, (int)position.y), newObject);
            return newObject;
        }

        public override void RemoveOldCells(List<Vector2> cellsOnScreen)
        {
            var positions = blocksTaken.Keys.ToArray();
            foreach (var pos in positions)
            {
                if (IsInBounds(pos, 10))
                    continue;

                Destroy(blocksTaken[pos].gameObject);
                blocksTaken.Remove(pos);
            }
        }

        private bool IsInBounds(Vector2 position, int area)
        {
            if (cameraRect.xMin - area > position.x)
                return false;
            if (cameraRect.xMax + area < position.x)
                return false;
            if (cameraRect.yMax + area < position.y)
                return false;
            if (cameraRect.yMin - area > position.y)
                return false;
            return true;
        }

        [System.Serializable]
        public struct LayerData
        {
            public TerrainLayer layer;
            public int height;
        }
    }
}
