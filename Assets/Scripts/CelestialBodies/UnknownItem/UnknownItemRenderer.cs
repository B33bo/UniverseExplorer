using UnityEngine;
using Universe.CelestialBodies;

namespace Universe
{
    public class UnknownItemRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new UnknownItem();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }
    }
}
