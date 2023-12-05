using UnityEngine;
using Universe.Terrain;

namespace Universe.CelestialBodies.Planets.Rocky
{
    public class RockyTerrainRenderer : PolyTerrainRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            meshRenderer.material.color *= ColorHighlights.Instance.primary;
        }

        public override void Spawn(Vector2 pos, int? seed, PolyTerrain previous, PolyTerrainLayer layer)
        {
            base.Spawn(pos, seed, previous, layer);
            meshRenderer.material.color *= ColorHighlights.Instance.primary;
        }
    }
}
