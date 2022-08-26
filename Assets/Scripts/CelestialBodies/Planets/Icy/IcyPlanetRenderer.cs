using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class IcyPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private SpriteRenderer[] icePanels;

        public override Type PlanetType => typeof(IcyPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            transform.localScale = GetFairSize((float)Target.Width, (float)IcyPlanet.MinScale, (float)IcyPlanet.MaxScale) * Vector2.one;

            for (int i = 0; i < icePanels.Length; i++)
            {
                Vector2 position = RandomNum.GetVector(-.5f, .5f, Target.RandomNumberGenerator);
                Vector2 scale = RandomNum.GetVector(.5f, 1, Target.RandomNumberGenerator);

                float Hue = RandomNum.GetFloat(165 / 360f, 250 / 360f, Target.RandomNumberGenerator);

                Color c = Color.HSVToRGB(Hue, 1, 1);
                c.a = Hue - (165 / 360f);

                icePanels[i].transform.localScale = scale;
                icePanels[i].transform.localPosition = position;
                icePanels[i].color = c;
            }
        }
    }
}
