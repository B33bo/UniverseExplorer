using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universe.CelestialBodies.Biomes;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanetRenderer : PlanetRenderer
    {
        private float perlinScale;
        private Vector2 perlinStart;
        private bool isLoading = false;
        private List<Continent.ContinentType> continentsClaimed;

        public override Type PlanetType => typeof(TerrestrialPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            perlinScale = RandomNum.GetFloat(5, 10, Target.RandomNumberGenerator);
            perlinStart = RandomNum.GetVector(-100_000, 100_000, Target.RandomNumberGenerator);
            Scale = GetFairSize((float)Target.Width, (float)TerrestrialPlanet.MinScale, (float)TerrestrialPlanet.MaxScale) * Vector2.one;
            continentsClaimed = new List<Continent.ContinentType>();

            TerrestrialPlanet t = Target as TerrestrialPlanet;
            for (int i = 0; i < t.continents.Length; i++)
            {
                if (!continentsClaimed.Contains(t.continents[i].continentType))
                    continentsClaimed.Add(t.continents[i].continentType);
            }

            StartCoroutine(GenerateSprites());
        }

        private Color GetColorOfTerrain(Vector2 perlinPos)
        {
            const float waterLevel = .5f;
            const float snowLevel = .9f;

            perlinPos += perlinStart * 2;
            float perlin = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);

            if (perlin < waterLevel)
                return Color.blue;
            if (perlin < waterLevel + .05f && continentsClaimed.Contains(Continent.ContinentType.Sand))
                return Color.yellow;
            else if (perlin > snowLevel && continentsClaimed.Contains(Continent.ContinentType.Snow))
                return Color.white;
            return Color.green;
        }

        private IEnumerator LoadSprite(int scale)
        {
            if (isLoading)
                yield break;
            isLoading = true;
            Vector2 half = new Vector2(scale * .5f, scale * .5f);
            Texture2D texture = new Texture2D(scale, scale);
            int circleRadSquared = (scale * scale) / 4;

            float normalizer = perlinScale / scale;

            for (int x = 0; x < scale; x++)
            {
                for (int y = 0; y < scale; y++)
                {
                    Vector2 iterator = new(x - half.x, y - half.y);

                    if (iterator.sqrMagnitude > circleRadSquared)
                    {
                        texture.SetPixel(x, y, Color.clear);
                        continue;
                    }

                    Vector2 perlinPos = iterator * normalizer + perlinStart;
                    Color c = GetColorOfTerrain(perlinPos);

                    texture.SetPixel(x, y, c);
                }

                yield return new WaitForEndOfFrame();
            }

            if (IsDestroyed)
                yield break;

            texture.filterMode = FilterMode.Trilinear;
            texture.Apply();
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
            sprite.sprite = newSprite;
            isLoading = false;
        }

        private IEnumerator GenerateSprites()
        {
            for (int i = 16; i <= 512; i *= 2)
            {
                yield return LoadSprite(i);

                while (IsLowRes)
                    yield return new WaitForEndOfFrame();

                if (IsDestroyed)
                    yield break;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
