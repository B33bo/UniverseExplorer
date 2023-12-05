using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Universe.Terrain
{
    public class PolygonalTerrainGenerator : Spawner
    {
        [SerializeField]
        private PolyTerrainLayer[] layers;

        [SerializeField]
        private bool colorBottomGround;

        [SerializeField]
        private SpriteRenderer bottomGround;

        private Dictionary<float, PolyTerrainRenderer[]> terrain;

        private float minimumY;

        public override void OnStart()
        {
            base.OnStart();
            CameraControl.Instance.OnPositionUpdate += UpdateBottomFloor;
            terrain = new Dictionary<float, PolyTerrainRenderer[]>();
            minimumY = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                minimumY -= layers[i].MinimumHeight;
                layers[i].Offset += (float)new System.Random(BodyManager.GetSeed()).NextDouble() * 1000 - 500;
            }

            bottomGround.transform.position = new Vector3(0, minimumY);

            if (colorBottomGround)
                bottomGround.color = ColorHighlights.Instance.primary;
        }

        private void UpdateBottomFloor(Rect bounds)
        {
            float scaleDownwards = Mathf.Min(minimumY - bounds.yMin, bounds.height) * 1.5f;
            if (scaleDownwards < 0) scaleDownwards = 0;

            bottomGround.transform.localScale = new Vector3(bounds.width * 1.5f, scaleDownwards);
            bottomGround.transform.position = new Vector3(bounds.center.x, Mathf.Min(bounds.yMax + .5f, minimumY));
        }

        public override void Destroyed()
        {
            CameraControl.Instance.OnPositionUpdate -= UpdateBottomFloor;
        }

        public override void GenerateNewCells(List<Vector2> cellsOnScreen)
        {
            float xMin = CameraControl.Instance.CameraBounds.xMin;
            float xMax = CameraControl.Instance.CameraBounds.xMax;

            xMin = Mathf.Floor(xMin / PolyTerrain.RealWidth) * PolyTerrain.RealWidth;
            xMax = Mathf.Ceil(xMax / PolyTerrain.RealWidth) * PolyTerrain.RealWidth;

            xMin -= PolyTerrain.RealWidth;
            xMax += PolyTerrain.RealWidth;

            SpawnCells(xMin, xMax);

            xMin -= PolyTerrain.RealWidth;
            xMax += PolyTerrain.RealWidth;

            RemoveCells(xMin, xMax);
        }

        private void RemoveCells(float xMin, float xMax)
        {
            float[] xPositions = terrain.Keys.ToArray();

            for (int i = 0; i < xPositions.Length; i++)
            {
                if (xPositions[i] > xMin && xPositions[i] < xMax)
                    continue;

                for (int j = 0; j < terrain[xPositions[i]].Length; j++)
                    Destroy(terrain[xPositions[i]][j].gameObject);
                terrain.Remove(xPositions[i]);
            }
        }

        private void SpawnCells(float xMin, float xMax)
        {
            float lastVal = float.NaN;
            float xPos = xMin;

            while (xPos <= xMax)
            {
                if (xPos == lastVal)
                {
                    Debug.LogWarning("uh oh floating point screw ups");
                    xPos = NextAfter.Next(xPos);
                }
                lastVal = xPos;

                if (terrain.ContainsKey(xPos))
                {
                    xPos += PolyTerrain.RealWidth;
                    continue;
                }

                PolyTerrainRenderer[] layerObjects = new PolyTerrainRenderer[layers.Length];
                float yVal = minimumY;

                PolyTerrain polyTerrain = null;

                for (int i = layers.Length - 1; i >= 0; i--)
                {
                    PolyTerrainRenderer terrainRenderer = SpawnLayer(i, xPos, yVal, polyTerrain);
                    layerObjects[i] = terrainRenderer;
                    polyTerrain = terrainRenderer.Target as PolyTerrain;
                    yVal += layers[i].MinimumHeight;
                }

                terrain.Add(xPos, layerObjects);

                xPos += PolyTerrain.RealWidth;
            }
        }

        private PolyTerrainRenderer SpawnLayer(int index, float x, float y, PolyTerrain previous)
        {
            var layer = Instantiate(layers[index].Renderer);
            layer.Spawn(new Vector2(x, y), new Vector2(x, y).HashPos(BodyManager.GetSeed()), previous, layers[index]);
            layer.Target.Name = $"Layer {x}:" + index;
            return layer;
        }

        public override List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            return null;
        }
    }
}
