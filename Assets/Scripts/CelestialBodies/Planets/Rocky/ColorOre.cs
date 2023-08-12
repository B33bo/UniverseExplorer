using UnityEngine;

namespace Universe.Blocks
{
    public class ColorOre : OreBlockRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            rockSpriteRenderer.color = ColorHighlights.Instance.primary;
        }

        public override void Spawn(Vector2 pos, int? seed, OreType oreType, Sprite rockSprite)
        {
            base.Spawn(pos, seed, oreType, rockSprite);
            rockSpriteRenderer.color = ColorHighlights.Instance.primary;
        }
    }
}
