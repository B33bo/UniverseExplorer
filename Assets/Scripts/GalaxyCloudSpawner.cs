using System.Collections.Generic;
using UnityEngine;

namespace Universe
{
    public class GalaxyCloudSpawner : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer galaxyCloud;

        private int seed;

        public const float CellSize = 2;

        private void Start()
        {
            seed = BodyManager.GetSeed();
        }

        private List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            const float cellSizeMultiplier = 3;

            Vector2 topLeft = new Vector2(cameraBounds.xMin - CellSize * cellSizeMultiplier, cameraBounds.yMax + CellSize * cellSizeMultiplier);
            Vector2 bottomRight = new Vector2(cameraBounds.xMax + CellSize * cellSizeMultiplier, cameraBounds.yMin - CellSize * cellSizeMultiplier);
            List<Vector2> cells = new List<Vector2>();

            topLeft.x -= topLeft.x % CellSize;
            bottomRight.y -= bottomRight.y % CellSize;

            for (float x = topLeft.x; x < bottomRight.x; x += CellSize)
            {
                for (float y = bottomRight.y; y < topLeft.y; y += CellSize)
                {
                    cells.Add(new Vector2(x, y));
                }
            }
            return cells;
        }
    }
}
