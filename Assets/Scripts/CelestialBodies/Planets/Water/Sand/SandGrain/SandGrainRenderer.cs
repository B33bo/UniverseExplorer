using UnityEngine;
using Universe.CelestialBodies.Planets.Water;

namespace Universe
{
    public class SandGrainRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private PolygonCollider2D collision;

        private void Start()
        {
            Spawn(Vector2.zero, 0);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            SandGrain sandGrain = new SandGrain();
            Target = sandGrain;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(Vector2.zero);

            meshFilter.mesh = sandGrain.mesh;
            collision.points = sandGrain.mesh.vertices.ToVector2();
        }
    }
}
