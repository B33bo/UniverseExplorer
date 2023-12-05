using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class RockyPlanetColorer : ColorHighlights
    {
        [SerializeField]
        private DayNightSystem dayNightSystem;

        [SerializeField]
        private SpriteRenderer bottomGround;

        protected override void OnAwake()
        {
            ColorHSV color;

            if (BodyManager.Parent is RockyPlanet rockyPlanet)
            {
                primary = rockyPlanet.RockColor;
                color = rockyPlanet.RockColor;
            }
            else
                color = primary;

            bottomGround.color *= primary;
            dayNightSystem.dayColor = new ColorHSV(color.h, color.s - .1f, color.v - .2f);
            dayNightSystem.nightColor = new ColorHSV(color.h, color.s - .1f, color.v - .5f);
        }
    }
}
