using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class GalaxySectionRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private bool rainbow = false;

        public override void Spawn(Vector2 pos, int? seed)
        {
            GalaxySection galaxySection = new GalaxySection();
            Target = galaxySection;

            if (seed.HasValue)
                galaxySection.SetSeed(seed.Value);

            Color a = Color.red, b = Color.blue;

            if (BodyManager.Parent is SpiralGalaxy galaxy)
            {
                a = galaxy.outer;
                b = galaxy.inner;
                rainbow = galaxy.IsRainbow;
            }

            ColorHSV aHSV = a;
            ColorHSV bHSV = b;
            aHSV.v = .25f;
            bHSV.v = .25f;
            a = aHSV;
            b = bHSV;

            galaxySection.Create(pos, 64, transform.localScale.x, a, b, rainbow);
            StartCoroutine(LoadSprite(galaxySection));
        }

        private IEnumerator LoadSprite(GalaxySection section)
        {
            for (int res = 16; res <= 512; res *= 2)
            {
                section.resolution = res;
                yield return section.GenerateTexture();
                Sprite sp = Sprite.Create(section.texture, new Rect(0, 0, section.texture.width, section.texture.height), Vector2.zero, section.texture.width);
                spriteRenderer.sprite = sp;
            }
        }
    }
}
