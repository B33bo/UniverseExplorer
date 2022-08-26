using UnityEngine;
using Universe.CelestialBodies.Biomes.Grass;

namespace Universe
{
    public class GrassRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override void Spawn(Vector2 pos, int? seed)
        {
            var grass = new Grass();
            Target = grass;
            if (seed.HasValue)
                grass.SetSeed(seed.Value);
            grass.Create(pos);
            spriteRenderer.color = grass.colorOffset;
        }
    }
}
