using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class RockyPlanetColorer : ColorHighlights
    {
        [SerializeField]
        private DayNightSystem dayNightSystem;

        protected override void OnAwake()
        {
            if (!(BodyManager.Parent is RockyPlanet rockyPlanet))
                return;
            primary = rockyPlanet.RockColor;

            dayNightSystem.dayColor = new ColorHSV(rockyPlanet.RockColor.h, rockyPlanet.RockColor.s - .1f, rockyPlanet.RockColor.v - .2f);
            dayNightSystem.nightColor = new ColorHSV(rockyPlanet.RockColor.h, rockyPlanet.RockColor.s - .1f, rockyPlanet.RockColor.v - .5f);
        }
    }
}
