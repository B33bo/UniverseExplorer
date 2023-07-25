using UnityEngine;

namespace Universe.Blocks
{
    public class IronBlockRenderer : BasicBlockRenderer
    {
        public const float height = 200;

        [SerializeField]
        private SpriteRenderer layer;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);

            var color = layer.color;
            color.a = 1 - (-pos.y / height);
            layer.color = color;
        }
    }
}
