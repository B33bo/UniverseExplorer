using System.Collections.Generic;
using UnityEngine;

namespace Universe.Terrain
{
    public class SingleBlock : TerrainLayer
    {
        [SerializeField]
        private CelestialBodyRenderer block;

        public override void Generate(Vector2Int bottomLeft, Vector2Int topRight, ref Dictionary<Vector2Int, CelestialBodyRenderer> takenPositions)
        {
            for (int x = bottomLeft.x; x <= topRight.x; x++)
            {
                for (int y = bottomLeft.y; y <= topRight.y; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    SpawnAt(block, ((Vector2)position).HashPos(BodyManager.GetSeed()), position, ref takenPositions);
                }
            }
        }
    }
}
