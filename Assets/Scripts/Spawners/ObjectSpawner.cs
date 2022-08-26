using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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

        public override void ReloadCells(Rect cameraRect)
        {
            var cellsOnScreen = CellsOnScreen(cameraRect);
            var positions = PositionsByObjects.Keys.ToArray();

            foreach (var pos in positions)
            {
                if (cellsOnScreen.Contains(pos))
                    continue;

                var cellsInPosition = PositionsByObjects[pos];

                if (detectStars)
                {
                    if (cellsInPosition[0].TryGetComponent(out StarRenderer starRenderer))
                    {
                        starsLoaded.Remove(starRenderer.Target as Star);
                    }
                }

                for (int i = 0; i < cellsInPosition.Length; i++)
                    Destroy(cellsInPosition[i]);
                PositionsByObjects.Remove(pos);
            }

            for (int i = 0; i < cellsOnScreen.Count; i++)
            {
                if (PositionsByObjects.ContainsKey(cellsOnScreen[i]))
                    continue;

                var spawned = SpawnAt(cellsOnScreen[i]);

                if (spawned.Length == 0)
                    continue;

                if (detectStars)
                {
                    if (spawned[0].TryGetComponent(out StarRenderer starRenderer))
                        starsLoaded.Add(starRenderer.Target as Star);
                }
            }
        }
    }
}
