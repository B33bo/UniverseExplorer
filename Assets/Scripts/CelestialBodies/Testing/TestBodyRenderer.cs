using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class TestBodyRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new TestBody(pos, seed.HasValue ? seed.Value : 0, "TestLand");
            Target.Create(pos);
        }
    }
}
