using UnityEngine;
using Universe.CelestialBodies.Planets.Molten;

namespace Universe
{
    public class VolcanoRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer Rock;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Volcano();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            pos.y += .85f;
            Target.Position = pos;

            if (ColorHighlights.Instance)
                Rock.color = ColorHighlights.Instance.primary;
        }
    }
}
