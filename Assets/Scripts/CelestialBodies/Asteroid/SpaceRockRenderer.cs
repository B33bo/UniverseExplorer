using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class SpaceRockRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new SpaceRock();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            if (BodyManager.Parent is SpaceRock)
                Destroy(GetComponent<Orbiter>());
            else
                GetComponent<Orbiter>().Activate(RandomNum.GetFloat(0f, 360, Target.RandomNumberGenerator), Target.Position.x, RandomNum.Get(0, 25, Target.RandomNumberGenerator));

            meshFilter.mesh = ShapeMaker.NormalizeMesh(ShapeMaker.RandomizeMesh(
                ShapeMaker.GetRegularShape(
                    RandomNum.Get(3, 30, Target.RandomNumberGenerator), .5f), 1, Target.RandomNumberGenerator)
                , out Vector3 bottomLeft, out Vector3 topRight);

            GetComponent<PolygonCollider2D>().points = meshFilter.mesh.vertices.ToVector2();

            Target.Width = topRight.x - bottomLeft.x;
            Target.Height = topRight.y - bottomLeft.y;

            double radius = (Target.Width + Target.Height) / 2;
            if (radius < 2.5f)
                (Target as SpaceRock).type = SpaceRock.Type.Meteoroid;
            if (radius < .5f)
                Destroy(gameObject);
        }
    }
}
