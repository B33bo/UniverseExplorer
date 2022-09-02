using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Universe
{
    public abstract class Spawner : MonoBehaviour
    {
        public float CellSize = 2;

        public bool registerSceneLoad = true;

        public Dictionary<Vector2, GameObject[]> PositionsByObjects;

        public CelestialBodyRenderer[] objects;
        public float[] weights;

        private bool subscribedToOnPositionUpdate = false;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (registerSceneLoad)
            {
                BodyManager.InvokeSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                BodyManager.ReloadCommands();
            }

            PositionsByObjects = new Dictionary<Vector2, GameObject[]>();

            if (weights.Length != objects.Length)
                Debug.LogError($"{name}: weights.length ({weights.Length}) != objects.length {objects.Length}");

            OnStart();
            ReloadCells(CameraControl.Instance.CameraBounds);
        }

        public virtual void OnStart()
        {
            CameraControl.Instance.OnPositionUpdate += ReloadCells;
            subscribedToOnPositionUpdate = true;
        }

        private void OnDestroy()
        {
            if (subscribedToOnPositionUpdate)
                CameraControl.Instance.OnPositionUpdate -= ReloadCells;

            Destroyed();
        }

        public virtual void ReloadCells(Rect cameraRect)
        {
            var cellsOnScreen = CellsOnScreen(cameraRect);
            var positions = PositionsByObjects.Keys.ToArray();

            foreach (var pos in positions)
            {
                if (cellsOnScreen.Contains(pos))
                    continue;

                var cellsInPosition = PositionsByObjects[pos];
                for (int i = 0; i < cellsInPosition.Length; i++)
                {
                    Debug.Log($"Destroying {cellsInPosition[i].name}");
                    Destroy(cellsInPosition[i]);
                }
                PositionsByObjects.Remove(pos);
            }

            for (int i = 0; i < cellsOnScreen.Count; i++)
            {
                if (PositionsByObjects.ContainsKey(cellsOnScreen[i]))
                    continue;

                SpawnAt(cellsOnScreen[i]);
            }
        }

        public virtual GameObject[] SpawnAt(Vector2 position)
        {
            if (objects.Length == 0)
                return null;

            var target = objects[GetSeededIndex(position, weights)];

            if (target is null)
                return System.Array.Empty<GameObject>();

            Debug.Log($"Spawning {target.name} at {position}");
            CelestialBodyRenderer newObject = Instantiate(target, position, Quaternion.identity);
            newObject.Spawn(position, null);

            float CellSizeRadius = CellSize / 2;
            newObject.Target.Position += (Vector3)RandomNum.GetVector(-CellSizeRadius, CellSizeRadius,
                                                                      newObject.Target.RandomNumberGenerator);

            var spawnedObjects = new GameObject[] { newObject.gameObject };
            PositionsByObjects.Add(position, spawnedObjects);
            return spawnedObjects;
        }

        public virtual List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            const float cellSizeMultiplier = 3;

            Vector2 topLeft = new Vector2(cameraBounds.xMin - CellSize * cellSizeMultiplier, cameraBounds.yMax + CellSize * cellSizeMultiplier);
            Vector2 bottomRight = new Vector2(cameraBounds.xMax + CellSize * cellSizeMultiplier, cameraBounds.yMin - CellSize * cellSizeMultiplier);

            topLeft.x -= topLeft.x % CellSize;
            bottomRight.y -= bottomRight.y % CellSize;

            List<Vector2> value = new List<Vector2>();

            float previousX = float.NaN;
            for (float x = topLeft.x; x < bottomRight.x; x += CellSize)
            {
                if (x == previousX)
                {
                    Debug.LogError("Out of valid world generation");
                    break;
                }

                previousX = x;
                float previousY = float.NaN;

                for (float y = bottomRight.y; y < topLeft.y; y += CellSize)
                {
                    if (y == previousY)
                    {
                        Debug.LogError("Out of valid world generation");
                        break;
                    }

                    previousY = y;
                    value.Add(new Vector2(x, y));
                }
            }

            return value;
        }

        public virtual List<Vector2> CellsWithinCircle(float radius)
        {
            Vector2 topLeft = -(Vector2.one * radius);
            Vector2 bottomRight = Vector2.one * radius;
            float radiusSqrd = radius * radius;

            List<Vector2> value = new List<Vector2>();

            for (float x = topLeft.x; x < bottomRight.x; x += CellSize)
            {
                for (float y = topLeft.y; y < bottomRight.y; y += CellSize)
                {
                    if (x * x + y * y < radiusSqrd)
                        value.Add(new Vector2(x, y));
                }
            }
            return value;
        }

        public int GetSeededIndex(Vector2 pos, float[] weights)
        {
            float weightSum = 0;
            for (int i = 0; i < weights.Length; i++)
                weightSum += weights[i];

            int seed = BodyManager.GetSeed();
            int positionSeed = (int)pos.x + (int)Mathf.Pow(pos.y, 3) + seed;
            float rndNum = RandomNum.GetFloat(weightSum, new System.Random(positionSeed));
            int randomIndex = RandomNum.GetIndexFromWeight(weights, rndNum);

            return randomIndex;
        }

        public virtual void Destroyed() { }
    }
}
