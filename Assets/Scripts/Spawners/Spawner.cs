using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Universe
{
    public abstract class Spawner : MonoBehaviour
    {
        public static int MaxCells = 10000;
        public static Spawner Instance { get; private set; }
        public float CellSize = 2;

        public bool registerSceneLoad = true;

        public Dictionary<Vector2, GameObject[]> PositionsByObjects;

        public CelestialBodyRenderer[] objects;
        public float[] weights;

        private bool subscribedToOnPositionUpdate = false;

        [SerializeField]
        private bool randomOffset = true;

        [SerializeField]
        private float extraPadding = 3;

        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (registerSceneLoad)
            {
                BodyManager.RegisterSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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

        private Vector2 positionOfLastReload = new(200, 200);
        public virtual void ReloadCells(Rect cameraRect)
        {
            if ((cameraRect.position - positionOfLastReload).sqrMagnitude < CellSize * CellSize)
                return;
            positionOfLastReload = cameraRect.position;
            var cellsOnScreen = CellsOnScreen(cameraRect);

            RemoveOldCells(cellsOnScreen);
            GenerateNewCells(cellsOnScreen);
        }

        public virtual void GenerateNewCells(List<Vector2> cellsOnScreen)
        {
            if (cellsOnScreen.Count > MaxCells)
                return;
            for (int i = 0; i < cellsOnScreen.Count; i++)
            {
                if (PositionsByObjects.ContainsKey(cellsOnScreen[i]))
                    continue;
                SpawnAt(cellsOnScreen[i]);
            }
        }

        public virtual void RemoveOldCells(List<Vector2> cellsOnScreen)
        {
            var positions = PositionsByObjects.Keys.ToArray();
            foreach (var pos in positions)
            {
                if (cellsOnScreen.Contains(pos))
                    continue;

                var cellsInPosition = PositionsByObjects[pos];
                for (int i = 0; i < cellsInPosition.Length; i++)
                {
                    Destroy(cellsInPosition[i]);
                }
                PositionsByObjects.Remove(pos);
            }
        }

        public virtual GameObject[] SpawnAt(Vector2 position)
        {
            if (objects.Length == 0)
                return null;

            int seed = BodyManager.GetSeed();
            int positionSeed = position.HashPos(seed);

            var rand = new System.Random(positionSeed);

            var target = objects[GetSeededIndex(weights, rand)];

            if (target is null)
                return Array.Empty<GameObject>();

            var newObject = SpawnAt(target, position, positionSeed);
            if (newObject == null)
                return null;

            float CellSizeRadius = CellSize / 2;

            if (randomOffset)
                newObject.Target.Position += (Vector3)RandomNum.GetVector(-CellSizeRadius, CellSizeRadius, rand);

            var spawnedObjects = new GameObject[] { newObject.gameObject };
            return spawnedObjects;
        }

        public virtual CelestialBodyRenderer SpawnAt(CelestialBodyRenderer prefab, Vector2 position, int? seed)
        {
            if (prefab is null)
                return null;
            if (CameraControl.Instance.MyCamera.orthographicSize > prefab.LowResScale)
                return null;

            CelestialBodyRenderer newObject = Instantiate(prefab, position, Quaternion.identity);
            newObject.Spawn(position, seed);
            PositionsByObjects.Add(position, new GameObject[] { newObject.gameObject });
            return newObject;
        }

        public virtual Vector2 GetTopLeft(Rect cameraBounds)
        {
            return new(cameraBounds.xMin - CellSize * extraPadding, cameraBounds.yMax + CellSize * extraPadding);
        }

        public virtual Vector2 GetBottomRight(Rect cameraBounds)
        {
            return new(cameraBounds.xMin - CellSize * extraPadding, cameraBounds.yMax + CellSize * extraPadding);
        }

        public virtual List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            Vector2 topLeft = GetTopLeft(cameraBounds);
            Vector2 bottomRight = GetBottomRight(cameraBounds);

            topLeft.x -= topLeft.x % CellSize;
            bottomRight.y -= bottomRight.y % CellSize;

            List<Vector2> value = new();

            float previousX = float.NaN;
            for (float x = topLeft.x; x < bottomRight.x; x += CellSize)
            {
                if (x == previousX)
                    x = NextAfter.NextSigned(x);

                previousX = x;
                float previousY = float.NaN;

                for (float y = bottomRight.y; y < topLeft.y; y += CellSize)
                {
                    if (y == previousY)
                        y = NextAfter.NextSigned(y);

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

            List<Vector2> value = new();

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

        public int GetSeededIndex(float[] weights, System.Random rand)
        {
            float weightSum = 0;
            for (int i = 0; i < weights.Length; i++)
                weightSum += weights[i];

            float rndNum = RandomNum.GetFloat(weightSum, rand);
            int randomIndex = RandomNum.GetIndexFromWeights(weights, rndNum);

            return randomIndex;
        }

        public virtual void Destroyed() { }
    }
}
