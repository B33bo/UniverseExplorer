using UnityEngine;

namespace Universe.Blocks
{
    public class ColorRockRenderer : BasicBlockRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            spriteRenderer.color = ColorHighlights.Instance.primary;
        }
    }
}
