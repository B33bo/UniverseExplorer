using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class RockyPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private SpriteRenderer[] rocks;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override Type PlanetType => typeof(RockyPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            transform.localScale = GetFairSize((float)Target.Width, RockyPlanet.MinScale, RockyPlanet.MaxScale) * Vector2.one;

            (float H, float S, float V) rockCol = (Target as RockyPlanet).RockColor;

            spriteRenderer.color = Color.HSVToRGB(rockCol.H, rockCol.S, rockCol.V);
            for (int i = 0; i < rocks.Length; i++)
            {
                rocks[i].transform.localScale = RandomNum.GetVector(.25f, .75f, Target.RandomNumberGenerator);
                rocks[i].transform.localPosition = RandomNum.GetVector(-.25f, .25f, Target.RandomNumberGenerator);

                float v = rockCol.V + RandomNum.GetFloat(.5f, Target.RandomNumberGenerator);
                v = Mathf.Clamp(v, 0, 1);
                Color c = Color.HSVToRGB(rockCol.H, rockCol.S, v);
                rocks[i].color = c;
            }
        }
    }
}
