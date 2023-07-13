using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class ObjectSpawner : Spawner
    {
        [SerializeField]
        private bool detectStars = false;

        public static List<Star> starsLoaded = new List<Star>();

        public override void OnStart()
        {
            base.OnStart();
            if (detectStars)
                starsLoaded = new List<Star>();
        }

        public override void RemoveOldCells(List<Vector2> cellsOnScreen)
        {
            var positions = PositionsByObjects.Keys.ToArray();
            foreach (var pos in positions)
            {
                if (cellsOnScreen.Contains(pos))
                    continue;

                var cellsInPosition = PositionsByObjects[pos];
                for (int i = 0; i < cellsInPosition.Length; i++)
                {
                    if (detectStars)
                    {
                        if (cellsInPosition[0].TryGetComponent(out StarRenderer starRenderer))
                            starsLoaded.Remove(starRenderer.Target as Star);
                    }

                    Destroy(cellsInPosition[i]);
                }
                PositionsByObjects.Remove(pos);
            }
        }

        public override CelestialBodyRenderer SpawnAt(CelestialBodyRenderer prefab, Vector2 position, int? seed)
        {
            var result = base.SpawnAt(prefab, position, seed);
            if (detectStars && result.TryGetComponent(out StarRenderer starRenderer))
                starsLoaded.Add(starRenderer.Target as Star);
            return result;
        }

        public override GameObject[] SpawnAt(Vector2 position)
        {
            var result = base.SpawnAt(position);
            if (result.Length == 0)
                return result;

            if (detectStars && result[0].TryGetComponent(out StarRenderer starRenderer))
                starsLoaded.Add(starRenderer.Target as Star);
            return result;
        }
    }
}
