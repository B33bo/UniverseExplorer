using UnityEngine;

namespace Universe.CelestialBodies.Planets.Toxic
{
    public class ToxicColorer : ColorHighlights
    {
        [SerializeField]
        private DayNightSystem dayNight;

        protected override void OnAwake()
        {
            ColorHSV color;
            color = BodyManager.Parent is ToxicPlanet toxicPlanet ? toxicPlanet.ToxicColor : primary;
            primary = color;

            color.s -= .5f;
            color.v -= .1f;
            dayNight.dayColor = color;
            secondary = color;
            color = primary;
            color.v -= .8f;
            dayNight.nightColor = color;
            tertiary = color;
        }
    }
}
