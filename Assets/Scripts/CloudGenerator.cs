using System.Xml.Serialization;
using UnityEngine;

namespace Universe
{
    public class CloudGenerator : MonoBehaviour
    {
        public int seed, points;
        public float changeY, height;
        public float cutoff;
        public Vector2 PerlinOffset, PerlinScale;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
                return;
            System.Random r = new System.Random();
            var tex = GenerateCloud(r, 256 * Vector2Int.one, points, changeY, height, cutoff, PerlinOffset, PerlinScale);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            var spr = GetComponent<SpriteRenderer>();
            spr.sprite = sprite;
            transform.rotation = Quaternion.Euler(0, 0, r.Next(0, 360));
            spr.color = Color.HSVToRGB((float)r.NextDouble(), 1, 1);
        }

        public Texture2D GenerateCloud(System.Random rand, Vector2Int spriteScale, int pointCount, float changeY, float height, float cutoff, Vector2 perlinOffset, Vector2 perlinScale)
        {
            Vector2[] points = GeneratePoints(rand, pointCount, changeY);
            Texture2D texture = SolidColor(spriteScale.x, spriteScale.y, Color.clear);

            for (int x = 0; x < spriteScale.x; x++)
            {
                float percent = (float)x / spriteScale.x;
                int lower = (int)(percent * (pointCount - 1));
                int higher = lower + 1;

                float t = percent % (1f / (pointCount - 1)) * pointCount;
                Vector2 position = Vector2.Lerp(points[lower], points[higher], t);

                int y = (int)(position.y * spriteScale.y);
                GetStrip(ref texture, height, new Vector2Int(x, y), perlinOffset, perlinScale, cutoff);
            }

            texture.Apply();
            return texture;
        }

        private void GetStrip(ref Texture2D texture, float height, Vector2Int pixelPos, Vector2 perlinOffset, Vector2 perlinScale, float cutoff)
        {
            int trueHeight = (int)(height * texture.height);
            int start = Mathf.Max(0, pixelPos.y - trueHeight / 2);
            int end = Mathf.Min(texture.height, pixelPos.y + trueHeight / 2);
            int middle = (start + end) / 2;

            Vector2 realPos = new Vector2((float)pixelPos.x / texture.width, (float)start / texture.height);
            float change = 1f / texture.height;

            for (int y = start; y < middle; y++)
            {
                Vector2 perlinPosition = realPos * perlinScale + perlinOffset;
                float perlin = Mathf.PerlinNoise(perlinPosition.x, perlinPosition.y);
                float grad = ((float)y - start) / (middle - start);

                if (perlin < cutoff)
                    break;

                perlin -= cutoff;
                perlin /= (1 - cutoff);
                texture.SetPixel(pixelPos.x, y, new Color(1, 1, 1, perlin * grad));
                realPos.y += change;
            }

            for (int y = middle; y < end; y++)
            {
                Vector2 perlinPosition = realPos * perlinScale + perlinOffset;
                float perlin = Mathf.PerlinNoise(perlinPosition.x, perlinPosition.y);
                float grad = ((float)y - middle) / (end - middle);

                if (perlin < cutoff)
                    break;

                perlin -= cutoff;
                perlin /= (1 - cutoff);
                texture.SetPixel(pixelPos.x, y, new Color(1, 1, 1, perlin * grad));
                realPos.y += change;
            }
        }

        public Texture2D SolidColor(int width, int height, Color color)
        {
            var tex = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    tex.SetPixel(x, y, color);
            }

            return tex;
        }

        private Vector2[] GeneratePoints(System.Random rand, int pointCount, float changeY)
        {
            Vector2[] points = new Vector2[pointCount];
            float lengthPerPoint = 1 / (pointCount - 1);
            Vector2 current = Vector2.zero;
            current.y = .5f;

            for (int i = 0; i < points.Length; i++)
            {
                float change = (float)(rand.NextDouble() * 2 - 1);
                change *= changeY;

                if (current.y + change > 1)
                    change *= -1;
                if (current.y + change < 0)
                    change *= -1;

                current.y += change;
                points[i] = current;

                current.x += lengthPerPoint;
            }

            return points;
        }
    }
}
