using UnityEngine;

namespace Universe.Blocks
{
    public class DarkstoneRenderer : BasicBlockRenderer
    {
        [SerializeField]
        private SpriteRenderer lava;

        [SerializeField]
        private Gradient gradient;

        [SerializeField]
        private Sprite[] sprites;

        private float offset;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            lava.sprite = sprites[Target.RandomNumberGenerator.Next(0, 4)];
            lava.name = lava.sprite.name;
            sprites = null;
            offset = Mathf.PerlinNoise(pos.x * .1f, pos.y * .1f);
        }

        public override void OnUpdate()
        {
            float val = GlobalTime.Time * .1f + offset;
            lava.color = gradient.Evaluate(val % 1);
        }
    }
}
