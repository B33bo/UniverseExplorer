using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Universe.CelestialBodies
{
    public class OreRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private string type, travelTarget;

        [SerializeField]
        private Texture texture;

        [SerializeField]
        private MeshFilter[] filters;

        [SerializeField]
        private MeshRenderer[] renderers;

        [SerializeField]
        private PolygonCollider2D collision;

        [SerializeField]
        private Light2D oreLight;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Ore ore = new();

            if (seed.HasValue)
                ore.SetSeed(seed.Value);

            ore.Create(pos);
            ore.LoadRandomOre();

            Target = ore;

            oreLight.color = ore.OreColor;

            LoadPolygon(0, ore);
            for (int i = 1; i < filters.Length; i++)
            {
                if (!RandomNum.GetBool(.5f, Target.RandomNumberGenerator))
                {
                    Destroy(filters[i].gameObject);
                    renderers[i].enabled = false;
                    continue;
                }

                LoadPolygon(i, ore);
            }

            Vector2[] collider = new Vector2[filters[0].mesh.vertices.Length];
            for (int i = 0; i < filters[0].mesh.vertices.Length; i++)
                collider[i] = filters[0].mesh.vertices[i];
            collision.points = collider;
        }

        protected override void HighRes()
        {
            oreLight.enabled = true;
        }

        protected override void LowRes()
        {
            oreLight.enabled = false;
        }

        private void LoadPolygon(int index, Ore ore)
        {
            Mesh mesh = ShapeMaker.GetRegularShape(5, 1);
            Vector3[] verts = mesh.vertices;

            for (int i = 0; i < verts.Length; i++)
                verts[i] *= RandomNum.GetFloat(1, Target.RandomNumberGenerator);

            mesh.vertices = verts;

            filters[index].mesh = mesh;

            if (texture != null)
                renderers[index].material.mainTexture = texture;

            Material mat = renderers[index].material;
            mat.color = ore.OreColor;
        }
    }
}
