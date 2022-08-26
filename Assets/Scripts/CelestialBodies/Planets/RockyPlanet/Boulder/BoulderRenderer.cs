using UnityEngine;
using Universe.CelestialBodies;
using Universe.CelestialBodies.Planets;
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
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
            meshFilter.mesh = b.rock;

            if (FindObjectOfType<TerrainGenerator>())
            {
                var color = TerrainGenerator.Instance.BiomeAtPosition(Target.Position.x).groundColor;
                Color.RGBToHSV(color, out float H, out float S, out float V);

                S += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);
                V += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);

                b.color = Color.HSVToRGB(H, S, V);
                meshFilter.GetComponent<MeshRenderer>().material.color = b.color;
            }

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
