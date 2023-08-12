using UnityEngine;

namespace Universe.Blocks
{
    public class OreBlockRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        protected SpriteRenderer rockSpriteRenderer, oreSpriteRenderer;
        private bool isRainbow;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Spawn(pos, seed, OreType.Unknownium, null);
        }

        public virtual void Spawn(Vector2 pos, int? seed, OreType oreType, Sprite rockSprite)
        {
            OreBlock oreBlock = new OreBlock();
            Target = oreBlock;

            if (seed.HasValue)
                oreBlock.SetSeed(seed.Value);

            oreBlock.Create(pos, oreType);
            oreType = oreBlock.Ore; // incase it swapped :)
            oreSpriteRenderer.color = oreType.Color;
            isRainbow = oreType.Name == "Rainbonite";

            if (rockSprite)
                rockSpriteRenderer.sprite = rockSprite;
        }

        public override void OnUpdate()
        {
            if (isRainbow)
                oreSpriteRenderer.color = Color.HSVToRGB(GlobalTime.Time % 1, 1, 1);
        }
    }
}
