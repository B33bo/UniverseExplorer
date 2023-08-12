using UnityEngine;

namespace Universe.CelestialBodies.Planets.Toxic
{
    public class ToxicColorer : ColorHighlights
    {
        [SerializeField]
        private DayNightSystem dayNight;

        protected override void OnAwake()
        {
            if (!(BodyManager.Parent is ToxicPlanet toxicPlanet))
                return;
            primary = toxicPlanet.ToxicColor;

            ColorHSV color = primary;
            color.s -= .5f;
            color.v -= .1f;
            dayNight.dayColor = color;
            color = primary;
            color.v -= .6f;
            dayNight.nightColor = color;
        }
    }
}
