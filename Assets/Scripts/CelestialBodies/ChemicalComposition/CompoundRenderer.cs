using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class CompoundRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Compound();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
            Target.Name = name;
        }

        public void Spawn(Vector2 pos, int? seed, Chemical chemical)
        {
            Compound c = new Compound();

            if (seed.HasValue)
                c.SetSeed(seed.Value);

            c.Create(pos, chemical);
            c.Name = name;
        }
    }
}
