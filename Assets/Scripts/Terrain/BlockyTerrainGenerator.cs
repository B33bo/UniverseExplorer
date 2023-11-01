using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universe.Animals;

namespace Universe.Terrain
{
    public class BlockyTerrainGenerator : Spawner
    {
        public const int MaxBlocks = 5000;
        public float Steepness;
        public float Wideness = 1;

        [SerializeField]
        private CelestialBodyRenderer topBlock;

        [SerializeField]
        private LayerData[] layers;

        public Dictionary<Vector2Int, CelestialBodyRenderer> blocksTaken;
        private Rect cameraRect;
        private Dictionary<int, float> perlinCache = new Dictionary<int, float>();
        private Vector2 lastReload;
        private float maxDepth;

        [SerializeField]
        private bool alwaysRenderTop, generate = true;

        [SerializeField]
        private AnimalSpawner animalSpawner;

        [SerializeField]
        private int animalFrequency;

        public static BlockyTerrainGenerator Instance { get; private set; }

        public override void OnStart()
        {
            Instance = this;
            blocksTaken = new Dictionary<Vector2Int, CelestialBodyRenderer>();
            base.OnStart();

            for (int i = 0; i < layers.Length; i++)
                maxDepth += layers[i].height;
        }

        public override List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            return null;
        }

        public static CelestialBodyRenderer BlockAt(int x, int y)
        {
            if (Instance is null)
                return null;
            if (Instance.blocksTaken.TryGetValue(new Vector2Int(x, y), out CelestialBodyRenderer val))
                return val;
            return null;
        }

        public override void Destroyed()
        {
            Instance = null;
        }

        public override void ReloadCells(Rect cameraRect)
        {
            if (!generate)
                return;
            if ((cameraRect.position - lastReload).sqrMagnitude < .8f)
                return;
            this.cameraRect = cameraRect;
            lastReload = cameraRect.position;
            RemoveOldCells(null);
            GenerateNewCells(null);
        }

        public override void GenerateNewCells(List<Vector2> cellsOnScreen)
        {
            int xMin = Mathf.FloorToInt(cameraRect.xMin) - 3;
            int xMax = Mathf.CeilToInt(cameraRect.xMax) + 3;

            GenerateBetween(xMin, xMax);
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

                if (!IsInBounds(position, 2) && !alwaysRenderTop)
                    continue;

                Vector2Int spawnAtPos = new Vector2Int(position.x, position.y + 1);

                if (!blocksTaken.ContainsKey(spawnAtPos))
                    SpawnAtPos(spawnAtPos);

                if (blocksTaken.ContainsKey(position))
                    continue;

                var block = Instantiate(topBlock);
                int blockseed = ((Vector2)position).HashPos(BodyManager.GetSeed());
                block.Spawn(position, blockseed);
                blocksTaken.Add(position, block);

                position.y--;
                layers[0].layer.Generate(new Vector2Int(position.x, 1), position, ref blocksTaken);

                if (animalFrequency == 0)
                    animalFrequency = 1;
                if (i % animalFrequency == 0)
                    animalSpawner?.SpawnAnimalAt(position, blockseed);
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
            if (!generate)
                return;
            var positions = blocksTaken.Keys.ToArray();
            foreach (var pos in positions)
            {
                // only delete sideways
                if (pos.x > cameraRect.xMin - 2 && pos.x < cameraRect.xMax + 2)
                {
                    if (pos.y > -maxDepth)
                        continue;
                    if (pos.y > cameraRect.yMin - 2 && pos.y < cameraRect.yMax + 2)
                        continue;
                }

                Destroy(blocksTaken[pos].gameObject);
                blocksTaken.Remove(pos);
            }
        }

        public void GenerateBetween(int xMin, int xMax)
        {
            int cameraBottomDepth = -(int)cameraRect.yMin;
            int cameraTopDepth = -(int)cameraRect.yMax;

            if (alwaysRenderTop || cameraRect.yMax > 0)
                GenerateTopLayer(xMin, xMax);

            int depth = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].height == 0)
                    continue;
                int oreTopDepth = depth;
                int oreBottomDepth = depth + layers[i].height - 1;
                depth += layers[i].height;

                if (cameraTopDepth > oreBottomDepth)
                    continue;

                if (cameraBottomDepth < oreTopDepth)
                    return;

                layers[i].layer.Generate(new Vector2Int(xMin, -oreBottomDepth), new Vector2Int(xMax, -oreTopDepth), ref blocksTaken);
            }

            if (depth > cameraBottomDepth)
                return;
            int finishLayerTop = depth;
            if (cameraTopDepth > finishLayerTop)
                finishLayerTop = cameraTopDepth;

            Vector2Int finishMin = new Vector2Int(xMin, -(cameraBottomDepth + 3));
            Vector2Int finishMax = new Vector2Int(xMax, -finishLayerTop);
            layers[layers.Length - 1].layer.Generate(finishMin, finishMax, ref blocksTaken);
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
