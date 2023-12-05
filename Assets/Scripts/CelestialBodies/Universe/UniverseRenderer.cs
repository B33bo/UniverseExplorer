using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class UniverseRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private PolygonCollider2D polygonCollider;

        [SerializeField]
        private GameObject lightObject;

        public override void Spawn(Vector2 position, int? seed)
        {
            Target = new Universe();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(position);

            float Size = GetFairSize((float)Target.Width, 1e35f, float.MaxValue);
            if (float.IsPositiveInfinity(Size))
                Size = 2.5f;
            Scale = Size * Vector2.one;

            StartCoroutine(LoadSprites());

            meshFilter.mesh = ShapeMaker.GetRegularShape((Target as Universe).Points, 5f / (Target as Universe).Points, true);
            meshRenderer.material.SetFloat("_SpeedX", RandomNum.GetFloat(.6f, Target.RandomNumberGenerator) - .3f);
            meshRenderer.material.SetFloat("_SpeedY", RandomNum.GetFloat(.6f, Target.RandomNumberGenerator) - .3f);
            meshRenderer.material.SetFloat("_Multiplier", RandomNum.GetFloat(.75f, Target.RandomNumberGenerator));

            Vector2[] colliderPoints = new Vector2[meshFilter.mesh.vertexCount];
            for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
            {
                colliderPoints[i] = meshFilter.mesh.vertices[i];
            }
            polygonCollider.points = colliderPoints;

            LowResScale = 50;
        }

        protected override void LowRes()
        {
            lightObject.SetActive(false);
        }

        protected override void HighRes()
        {
            lightObject.SetActive(true);
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
            meshRenderer.material.mainTexture = texture;
        }

        private IEnumerator LoadSprites()
        {
            for (int i = 4; i <= 64; i *= 2)
            {
                yield return LoadSprite(i, i);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
