using UnityEngine;
using Universe.CelestialBodies.Planets.Icy;

namespace Universe
{
    public class IceCrystalRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new IceCrystal();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            GenerateMesh();

            GetComponent<PolygonCollider2D>().points = meshFilter.mesh.vertices.ToVector2();
        }

        private void GenerateMesh()
        {
            float extension = (Target as IceCrystal).CrystalHeight;
            Mesh mesh = ShapeMaker.GetRegularShape(5, 1);
            Vector3[] verticies = mesh.vertices;
            verticies[1] += new Vector3(0, extension);
            verticies[2] += new Vector3(0, extension);
            verticies[3] += new Vector3(0, extension);
            mesh.vertices = verticies;

            cameraLerpTarget.localPosition = new Vector3(0, extension / 2);

            meshFilter.mesh = mesh;
        }
    }
}
