using UnityEngine;
using Universe.CelestialBodies.Biomes.Grass;

namespace Universe
{
    public class FlowerRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Sprite[] sprites;

        [SerializeField]
        private SpriteRenderer flowerSprite;

        public int seed;

        private void Awake()
        {
            Spawn(transform.position, seed);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Flower flower = new Flower();
            Target = flower;
            if (seed.HasValue)
                flower.SetSeed(seed.Value);
            flower.Create(pos);

            if (flower.type == Flower.Type.LilyOfTheValley)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[(int)flower.type];
                Destroy(flowerSprite.gameObject);
                return;
            }

            flowerSprite.sprite = sprites[(int)flower.type];
            flowerSprite.color = flower.color;
            flowerSprite.transform.localPosition += (Vector3)flower.offset;
        }
    }
}
