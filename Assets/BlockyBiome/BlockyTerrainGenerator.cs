using System.Collections.Generic;
using UnityEngine;

namespace Universe
{
    public class BlockyTerrainGenerator : Spawner
    {
        [SerializeField]
        private float maxBlockHeight;

        private int seed;

        public BlockLayer[] blockLayers;

        public override void OnStart()
        {
            base.OnStart();
            CellSize = 1;
            seed = BodyManager.GetSeed();
        }

        private BlockLayer GetLayerAt(Vector2 position)
        {
            BlockLayer closestLayer = blockLayers[0];
            float closestLayerDifference = float.PositiveInfinity;

            for (int i = 0; i < blockLayers.Length; i++)
            {
                float newDiff = Difference(blockLayers[i].Y, position.y);

                if (newDiff > closestLayerDifference)
                    continue;

                closestLayer = blockLayers[i];
                closestLayerDifference = newDiff;
            }

            return closestLayer;
        }

        private float Difference(float a, float b) =>
            b > a ? b - a : a - b;

        private float GetY(float x)
        {
            if (x % 10 == 0)
            {
                var rnd = new System.Random(new System.Random((int)(x * 300)).Next() + seed);
                if (x < 0)
                    rnd.NextDouble();
                return rnd.Next(0, (int)maxBlockHeight);
            }

            float left = Mathf.Floor(x / 10) * 10;
            float right = Mathf.Ceil(x / 10) * 10;

            if (left == x || right == x || left == right)
            {
                Debug.LogError("Ground cannot find correct Y level, defaulting to 0");
                return 0;
            }

            float t = (x - left) / (right - left);

            return (int)Mathf.Lerp(GetY(left), GetY(right), t);
        }

        public override void GenerateNewCells(List<Vector2> cellsOnScreen)
        {
            float y = float.NaN;
            BlockLayer blockLayer = null;

            for (int i = 0; i < cellsOnScreen.Count; i++)
            {
                if (PositionsByObjects.ContainsKey(cellsOnScreen[i]))
                    continue;

                float yTop = GetY(cellsOnScreen[i].x);
                if (cellsOnScreen[i].y > yTop)
                    continue;

                if (cellsOnScreen[i].y != y)
                {
                    blockLayer = GetLayerAt(cellsOnScreen[i]);
                    y = cellsOnScreen[i].y;
                }

                SpawnAt(blockLayer, cellsOnScreen[i], yTop);
            }
        }

        public override List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            List<Vector2> cells = new List<Vector2>();
            float previousY = float.NaN;

            Vector2 topLeft = new Vector2(cameraBounds.xMin - CellSize * 2, cameraBounds.yMax + CellSize * (maxBlockHeight + 2));
            Vector2 bottomRight = new Vector2(cameraBounds.xMax + CellSize * 2, cameraBounds.yMin - CellSize * (maxBlockHeight + 2));

            topLeft.x -= topLeft.x % CellSize;
            bottomRight.y -= bottomRight.y % CellSize;

            for (float y = bottomRight.y; y < topLeft.y; y += CellSize)
            {
                if (previousY == y)
                {
                    Debug.LogError("Out of valid world generation");
                    break;
                }

                previousY = y;

                float previousX = float.NaN;
                for (float x = topLeft.x; x < bottomRight.x; x += CellSize)
                {
                    cells.Add(new Vector2(x, y));

                    if (previousX == x)
                    {
                        Debug.LogError("Out of valid world generation");
                        break;
                    }

                    previousX = x;
                }
            }

            return cells;
        }

        public void SpawnAt(BlockLayer blockLayer, Vector2 position, float yTop)
        {
            if (blockLayer.renderTop)
            Debug.Log($"{yTop} {position.y} {blockLayer.renderTop && position.y == yTop}");
            if (blockLayer.renderTop && position.y == yTop)
                SpawnAt(blockLayer.top, position);
            else
                SpawnAt(blockLayer.block, position);
        }
    }
}
