using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class GalaxySection : CelestialBody
    {
        public override string TypeString => "Galaxy Region";

        public override string TravelTarget => "Galaxy";

        public override bool Circular => false;

        public Texture2D texture;
        private float scale;
        private Color a, b;
        private bool rainbow;
        public int resolution;

        public override void Create(Vector2 pos)
        {
            Create(pos, 64, 1, Color.clear, Color.white, false);
        }

        public void Create(Vector2 pos, int res, float scale, Color a, Color b, bool rainbow)
        {
            Position = pos;
            Name = "Cloud";
            this.scale = scale;
            this.a = a;
            this.b = b;
            this.rainbow = rainbow;
            resolution = res;

            Width = scale;
            Height = scale;

            GenerateTexture();
        }

        public IEnumerator GenerateTexture()
        {
            texture = new Texture2D(resolution, resolution)
            {
                filterMode = FilterMode.Point, // Looks weird otherwise idk why
            };

            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    Vector2 position = new Vector2(x / (float)(resolution) * scale, y / (float)(resolution) * scale) + (Vector2)Position;
                    float perlin = Mathf.PerlinNoise(position.x, position.y);

                    Color c = rainbow ? Color.HSVToRGB(perlin, 1, 1) : Color.Lerp(a, b, perlin);

                    texture.SetPixel(x, y, c);
                }
                yield return new WaitForEndOfFrame();
            }

            texture.Apply();
        }
    }
}
