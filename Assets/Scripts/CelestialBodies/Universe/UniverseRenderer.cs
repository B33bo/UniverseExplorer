using System.Collections;
using System.Collections.Generic;
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

        private static Dictionary<int, Mesh> meshesOfShapes = new();

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

            int res = Mathf.Abs(Target.Seed % 14) + 4;
            StartCoroutine(LoadSprite(res, res));

            meshFilter.mesh = GetMesh();
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

        private Mesh GetMesh()
        {
            int points = (Target as Universe).Points;

            if (!meshesOfShapes.ContainsKey(points))
                meshesOfShapes.Add(points, ShapeMaker.GetRegularShape(points, 5f / points));

            return meshesOfShapes[points];
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
            Texture2D texture = new Texture2D(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float R = RandomNum.GetFloat(1, Target.RandomNumberGenerator);
                    float G = RandomNum.GetFloat(1, Target.RandomNumberGenerator);
                    float B = RandomNum.GetFloat(1, Target.RandomNumberGenerator);

                    texture.SetPixel(i, j, new Color(R, G, B));
                }
                yield return new WaitForEndOfFrame();
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            meshRenderer.material.mainTexture = texture;
        }
    }
}
