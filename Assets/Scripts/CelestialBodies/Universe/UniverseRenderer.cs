using UnityEngine;
using System.Collections;

namespace Universe.CelestialBodies
{
    public class UniverseRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override void Spawn(Vector2 position, int? seed)
        {
            Target = new Universe();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(position);

            float Size = GetFairSize((float)Target.Width, 1e35f, float.MaxValue);
            if (float.IsPositiveInfinity(Size))
                Size = 2.5f;
            transform.localScale = Size * Vector2.one;

            StartCoroutine(LoadSprites());
        }

        private IEnumerator LoadSprite(int width, int height)
        {
            Vector2 halfScale = new Vector2(width, height) * .5f;
            Texture2D texture = new Texture2D(width, height);
            int circleRadiusSquared = (width * width) / 4;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector2 normalizedIterator = new Vector2(j - halfScale.x, i - halfScale.y);

                    float R = RandomNum.GetFloat(1, Target.RandomNumberGenerator);
                    float G = RandomNum.GetFloat(1, Target.RandomNumberGenerator);
                    float B = RandomNum.GetFloat(1, Target.RandomNumberGenerator);

                    if (normalizedIterator.sqrMagnitude < circleRadiusSquared)
                        texture.SetPixel(i, j, new Color(R, G, B));
                    else
                        texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
                yield return new WaitForEndOfFrame();
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            Sprite x = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
            spriteRenderer.sprite = x;
        }

        private IEnumerator LoadSprites()
        {
            for (int i = 4; i <= 128; i *= 2)
            {
                yield return LoadSprite(i, i);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
