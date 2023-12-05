using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe.CelestialBodies.Planets
{
    public class RockyPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private SpriteRenderer[] rocks;

        public override Type PlanetType => typeof(RockyPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Width, RockyPlanet.MinScale, RockyPlanet.MaxScale) * Vector2.one;

            ColorHSV rockCol = (Target as RockyPlanet).RockColor;

            sprite.color = rockCol;
            for (int i = 0; i < rocks.Length; i++)
            {
                rocks[i].transform.localScale = RandomNum.GetVector(.25f, .75f, Target.RandomNumberGenerator);
                rocks[i].transform.localPosition = RandomNum.GetVector(-.25f, .25f, Target.RandomNumberGenerator);

                float v = rockCol.v + RandomNum.GetFloat(.5f, Target.RandomNumberGenerator);
                v = Mathf.Clamp(v, 0, 1);
                rocks[i].color = new ColorHSV(rockCol.h, rockCol.s, v);
            }
        }

        protected override void HighRes()
        {
            base.HighRes();
            for (int i = 0; i < rocks.Length; i++)
                rocks[i].enabled = true;
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            base.LowRes();
            for (int i = 0; i < rocks.Length; i++)
                rocks[i].enabled = false;
        }
    }
}
