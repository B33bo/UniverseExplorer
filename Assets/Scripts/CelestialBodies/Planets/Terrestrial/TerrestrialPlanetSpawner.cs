using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanetSpawner : Spawner
    {
        private Vector2[] oldPositions;
        private TerrestrialPlanet planet;

        [SerializeField]
        private Color col;

        public override void OnStart()
        {
            CameraControl.Instance.MyCamera.backgroundColor = col;
            CameraControl.Instance.Position = Vector2.zero;

            if (!(BodyManager.Parent is TerrestrialPlanet terrestrialPlanet))
            {
                terrestrialPlanet = new TerrestrialPlanet();
                terrestrialPlanet.SetSeed(0);
                terrestrialPlanet.Create(Vector2.zero);
            }

            planet = terrestrialPlanet;
            Vector2 startPos = new Vector2(-(terrestrialPlanet.continents.Length - 1) / 2f, 0);
            ContinentRenderer continentRenderer = Resources.Load<ContinentRenderer>("Objects/Continent");
            oldPositions = new Vector2[terrestrialPlanet.continents.Length];

            for (int i = 0; i < terrestrialPlanet.continents.Length; i++)
            {
                var newCont = Instantiate(continentRenderer);
                newCont.Target = terrestrialPlanet.continents[i];
                oldPositions[i] = terrestrialPlanet.continents[i].Position;
                newCont.Target.Position = startPos + new Vector2(i, 0);
                newCont.Init();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < oldPositions.Length; i++)
            {
                planet.continents[i].Position = oldPositions[i];
            }
        }
    }
}
