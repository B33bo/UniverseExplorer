using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class OreRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private string type, travelTarget;

        [SerializeField]
        private Texture texture;

        [SerializeField]
        private Color color;

        [SerializeField]
        private MeshFilter[] filters;

        [SerializeField]
        private MeshRenderer[] renderers;

        [SerializeField]
        private PolygonCollider2D collision;

        private void Awake()
        {
            Spawn(Vector2.zero, 0);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Ore ore = new();

            if (seed.HasValue)
                ore.SetSeed(seed.Value);

            ore.Create(pos, type, travelTarget, color);
            Target = ore;

            LoadPolygon(0);
            for (int i = 1; i < filters.Length; i++)
            {
                if (!RandomNum.GetBool(.5f, Target.RandomNumberGenerator))
                {
                    Destroy(filters[i].gameObject);
                    continue;
                }
                LoadPolygon(i);
            }

            Vector2[] collider = new Vector2[filters[0].mesh.vertices.Length];
            for (int i = 0; i < filters[0].mesh.vertices.Length; i++)
                collider[i] = filters[0].mesh.vertices[i];
            collision.points = collider;
        }

        private void LoadPolygon(int index)
        {
            Mesh mesh = ShapeMaker.GetRegularShape(5, 1, true);
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] *= RandomNum.GetFloat(1, Target.RandomNumberGenerator);
            mesh.vertices = verts;

            filters[index].mesh = mesh;

            if (texture != null)
                renderers[index].material.mainTexture = texture;
            renderers[index].material.color = color;
        }
    }
}
