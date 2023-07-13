using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanetRenderer : PlanetRenderer
    {
        public TerrestrialPlanet TargetPlanet;
        public ContinentRenderer[] continents;

        public override Type PlanetType => typeof(TerrestrialPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            TargetPlanet = Target as TerrestrialPlanet;
            continents = new ContinentRenderer[TargetPlanet.continents.Length];

            Scale = GetFairSize((float)Target.Width, (float)TerrestrialPlanet.MinScale, (float)TerrestrialPlanet.MaxScale) * Vector2.one;
            ContinentRenderer continentRenderer = Resources.Load<ContinentRenderer>("Objects/Continent");
            for (int i = 0; i < continents.Length; i++)
            {
                continents[i] = Instantiate(continentRenderer, transform);
                continents[i].Target = TargetPlanet.continents[i];
                continents[i].Init();
                continents[i].Scale = new Vector3(.1f, .1f);
                Vector3 localPos = RandomNum.GetVector(-.5f, .5f, Target.RandomNumberGenerator);

                continents[i].Target.Position = localPos;

                if (BodyManager.Parent is Moon)
                    continents[i].CameraFocus = false;
            }

            GetComponent<CircleCollider2D>().enabled = !(BodyManager.Parent is Planet);
        }

        protected override void Destroyed()
        {
            for (int i = 0; i < continents.Length; i++)
            {
                if (continents[i].enabled)
                    Destroy(continents[i].gameObject);
            }
        }
    }
}
