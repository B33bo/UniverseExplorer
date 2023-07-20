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
                Vector3 localPos = Target.Seed == Star.Earth ? 
                    GetEarthContinentPos(i) :
                    RandomNum.GetVector(-.5f, .5f, Target.RandomNumberGenerator);

                continents[i].Target.Position = localPos;

                if (BodyManager.Parent is Moon)
                    continents[i].CameraFocus = false;
            }
        }

        private Vector2 GetEarthContinentPos(int index)
        {
            return index switch
            {
                TerrestrialPlanet.NorthAmerica => new Vector2(-.28f, 1.5f),
                TerrestrialPlanet.SouthAmerica => new Vector2(-.28f, -1.5f),
                TerrestrialPlanet.Africa => new Vector2(0, -.2f),
                TerrestrialPlanet.Europe => new Vector2(0, .3f),
                TerrestrialPlanet.Oceania => new Vector2(.5f, -.4f),
                TerrestrialPlanet.Antarctica => new Vector2(0, -.5f),
                _ => Vector2.zero
            };
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
