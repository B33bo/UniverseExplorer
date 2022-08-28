using UnityEngine;

namespace Universe.CelestialBodies.Terrain
{
    public class SandGroundRenderer : TerrainObject
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new SandGround();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }
    }
}
