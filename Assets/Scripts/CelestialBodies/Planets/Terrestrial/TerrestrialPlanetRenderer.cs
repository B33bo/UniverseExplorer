using System;
using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanetRenderer : PlanetRenderer
    {
        private float perlinScale;
        private Vector2 perlinStart;
        private bool isLoading = false;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override Type PlanetType => typeof(TerrestrialPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            perlinScale = RandomNum.GetFloat(5, 10, Target.RandomNumberGenerator);
            perlinStart = RandomNum.GetVector(-100_000, 100_000, Target.RandomNumberGenerator);
            Scale = GetFairSize((float)Target.Width, (float)TerrestrialPlanet.MinScale, (float)TerrestrialPlanet.MaxScale) * Vector2.one;
            StartCoroutine(GenerateSprites());

            Target.OnInspected += variable =>
            {
                if (variable.VariableName != "Sea Level")
                    return;
                StartCoroutine(LoadSprite(64));
            };
        }

        private IEnumerator LoadSprite(int scale)
        {
            if (isLoading)
                yield break;
            isLoading = true;
            Vector2 half = new Vector2(scale * .5f, scale * .5f);
            Texture2D texture = new Texture2D(scale, scale);
            int circleRadSquared = (scale * scale) / 4;
            const float waterLevel = .5f;

            float normalizer = perlinScale / scale;

            for (int x = 0; x < scale; x++)
            {
                for (int y = 0; y < scale; y++)
                {
                    Vector2 iterator = new Vector2(x - half.x, y - half.y);

                    if (iterator.sqrMagnitude > circleRadSquared)
                    {
                        texture.SetPixel(x, y, Color.clear);
                        continue;
                    }

                    Vector2 perlinPos = iterator * normalizer + perlinStart;
                    float perlin = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);

                    Color c;
                    if (perlin < waterLevel)
                        c = Color.blue;
                    else if (perlin < waterLevel + .05f)
                        c = Color.yellow;
                    else
                        c = new Color(0, perlin, 0);

                    texture.SetPixel(x, y, c);
                }

                yield return new WaitForEndOfFrame();
            }
            texture.filterMode = FilterMode.Trilinear;
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
            spriteRenderer.sprite = sprite;
            isLoading = false;
        }

        private IEnumerator GenerateSprites()
        {
            for (int i = 16; i <= 512; i *= 2)
            {
                yield return LoadSprite(i);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
