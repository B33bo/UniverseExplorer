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
                a = galaxy.Color.outer;
                b = galaxy.Color.inner;
                rainbow = galaxy.Rainbow;
            }

            galaxySection.Create(pos, 64, transform.localScale.x, a, b, rainbow);
            StartCoroutine(LoadSprite(galaxySection));
        }

        private IEnumerator LoadSprite(GalaxySection section)
        {
            for (int res = 4; res < 128; res *= 2)
            {
                section.resolution = res;
                yield return section.GenerateTexture();
                Sprite sp = Sprite.Create(section.texture, new Rect(0, 0, section.texture.width, section.texture.height), Vector2.zero, section.texture.width);
                spriteRenderer.sprite = sp;
            }
        }
    }
}
