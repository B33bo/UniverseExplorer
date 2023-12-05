using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class SeaGlassRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new SeaGlass();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            meshFilter.mesh = GetMesh();

            Target.OnInspected += ChangeVar;

            Color color = (Target as SeaGlass).color;
            color.a = .4f;
            GetComponent<MeshRenderer>().material.color = color;
            transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(360, Target.RandomNumberGenerator));
        }

        private void ChangeVar(Variable var)
        {
            meshFilter.mesh = GetMesh();
            Color color = (Target as SeaGlass).color;
            color.a = .4f;
            GetComponent<MeshRenderer>().material.color = color;
        }

        private Mesh GetMesh()
        {
            SeaGlass seaGlass = Target as SeaGlass;

            int points = seaGlass.Points * seaGlass.Smoothness;
            var shape = ShapeMaker.GetRegularShape(points, 3f / points);
            System.Random rand = new(seaGlass.Seed);

            Vector3[] verts = shape.vertices;
            Vector3 offset = Vector2.zero;
            for (int i = 0; i < verts.Length; i++)
            {
                if (i % seaGlass.Smoothness == 0)
                {
                    float x = RandomNum.GetFloat(-seaGlass.Shape, seaGlass.Shape, rand);
                    float y = RandomNum.GetFloat(-seaGlass.Shape, seaGlass.Shape, rand);
                    offset = new Vector2(x, y);
                }

                verts[i] += offset;
            }

            shape.vertices = verts;
            return shape;
        }
    }
}
