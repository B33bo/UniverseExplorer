using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Universe.CelestialBodies.Planets.Icy
{
    public class ChristmasTreeRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer[] baubles;

        [SerializeField]
        private Light2D[] baubleLights;

        [SerializeField]
        private MeshFilter tree;

        [SerializeField]
        private MeshRenderer treeLeaves;

        [SerializeField]
        private Texture2D treeTexture, tinselTexture;

        [SerializeField]
        private MeshFilter star;

        [SerializeField]
        private MeshRenderer starRenderer;

        [SerializeField]
        private Light2D starLight;

        [SerializeField]
        private MeshFilter tinsel;

        [SerializeField]
        private MeshRenderer tinselRenderer;

        [SerializeField]
        private Light2D glow;

        private static Mesh christmasTreeMesh;
        private static Dictionary<int, Mesh> starMesh = new();

        public override void Spawn(Vector2 pos, int? seed)
        {
            ChristmasTree christmasTree = new();
            Target = christmasTree;

            if (seed.HasValue)
                christmasTree.SetSeed(seed.Value);

            christmasTree.Create(pos);

            if (christmasTreeMesh == null)
                LoadChristmasTreeMesh();

            tree.mesh = christmasTreeMesh;
            treeLeaves.material.mainTexture = treeTexture;

            tinsel.mesh = christmasTreeMesh;
            tinselRenderer.material.color = christmasTree.tinselColor;
            tinselRenderer.material.mainTexture = tinselTexture;

            if (!starMesh.ContainsKey(christmasTree.starPoints))
                starMesh.Add(christmasTree.starPoints, ShapeMaker.GetStar(5, .2f, 2));
            star.mesh = starMesh[christmasTree.starPoints];

            starRenderer.material.color = christmasTree.starColor;
            starLight.color = christmasTree.starColor;

            glow.color = christmasTree.baubleColor1;

            LoadBaubles();
        }

        private void LoadBaubles()
        {
            for (int i = 0; i < baubles.Length; i++)
                LoadBauble(baubles[i], baubleLights[i]);
        }

        private void LoadBauble(SpriteRenderer bauble, Light2D light)
        {
            ChristmasTree christmasTree = Target as ChristmasTree;
            bauble.transform.localPosition = RandomPointOnTree(christmasTree);
            bauble.color = RandomNum.GetBool(Target.RandomNumberGenerator) ? christmasTree.baubleColor1 : christmasTree.baubleColor2;

            light.color = bauble.color;
        }

        private Vector2 RandomPointOnTree(ChristmasTree christmasTree)
        {
            Debug.Log(christmasTree.BranchWidths[^1]);
            System.Random rand = christmasTree.RandomNumberGenerator;
            int yIndex = RandomNum.Get(0, christmasTree.BranchWidths.Length - 1, rand);

            Vector2 min = christmasTree.BranchWidths[yIndex];
            Vector2 max = christmasTree.BranchWidths[yIndex + 1];

            float interpolation = (float)rand.NextDouble();

            float yPos = Mathf.Lerp(min.y, max.y, interpolation);
            float maxX = Mathf.Lerp(min.x, max.x, interpolation);

            float xPosition = RandomNum.GetFloat(-maxX, maxX, rand);
            return new Vector2(xPosition, yPos + .4f);
        }

        private void LoadChristmasTreeMesh()
        {
            ChristmasTree christmasTree = Target as ChristmasTree;
            int branches = christmasTree.BranchWidths.Length;
            Vector3[] verts = new Vector3[branches * 2];
            Vector2[] UVs = new Vector2[verts.Length];

            int[] tris = new int[branches * 3];

            for (int i = 0; i < branches; i++)
            {
                Vector2 vertPos = christmasTree.BranchWidths[i];
                verts[i] = vertPos;

                vertPos.x *= -1;
                verts[verts.Length - 1 - i] = vertPos;
                UVs[i] = new Vector2(0, i % 2);
                UVs[verts.Length - 1 - i] = new Vector2(1, i % 2);

                Debug.Log(i + " " + UVs[i]);
            }

            for (int i = 0; i < branches - 1; i += 2)
            {
                int triIndex = i * 3;
                tris[triIndex] = i;
                tris[triIndex + 1] = i + 1;
                tris[triIndex + 2] = verts.Length - 1 - i;

                tris[triIndex + 3] = i + 1;
                tris[triIndex + 4] = verts.Length - 2 - i;
                tris[triIndex + 5] = verts.Length - 1 - i;
            }

            christmasTreeMesh = new Mesh()
            {
                vertices = verts,
                triangles = tris,
                uv = UVs,
            };
        }
    }
}
