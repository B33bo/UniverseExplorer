using UnityEngine;
using Universe.CelestialBodies.Planets.Rocky;

namespace Universe
{
    public class BoulderRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Boulder b = new Boulder();
            Target = b;
            pos.y += .5f;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
            meshFilter.mesh = b.rock;

            Color color;

            if (ColorHighlights.Instance)
                color = Color.Lerp(ColorHighlights.Instance.primary, ColorHighlights.Instance.secondary, RandomNum.GetFloat(1, Target.RandomNumberGenerator));
            else
                color = new ColorHSV(0, 0, 0);

            b.color = color;
            meshFilter.GetComponent<MeshRenderer>().material.color = b.color;

            GetComponent<PolygonCollider2D>().points = b.rock.vertices.ToVector2();
        }

        public void SpawnBoulder(Vector2 pos, Mesh mesh, Color color, int index)
        {
            Boulder b = new Boulder();
            Target = b;
            b.Create(pos, mesh);
            b.Name = $"Rock #{index + 1}";
            meshFilter.mesh = mesh;
            meshFilter.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
