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
                Color color;
                if (BodyManager.Parent is RockyPlanet rockyPlanet)
                    color = rockyPlanet.RockColor;
                else
                     color = TerrainGenerator.Instance.BiomeAtPosition(Target.Position.x).groundColor;

                ColorHSV colorHSV = color;
                colorHSV.s += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);
                colorHSV.v += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);

                b.color = colorHSV;
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
