using System.Collections;
using UnityEngine;
using Universe.Animals;

namespace Universe.CelestialBodies.Planets.Grass
{
    public class FlowerRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Sprite[] sprites;

        [SerializeField]
        private SpriteRenderer flowerSprite;

        private ButterflyRenderer[] butterflies;

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

            StartCoroutine(InitButterflies());
        }

        private IEnumerator InitButterflies()
        {
            yield return new WaitForFrames(2);

            Flower flower = Target as Flower;
            butterflies = new ButterflyRenderer[flower.butterflyCount];
            for (int i = 0; i < butterflies.Length; i++)
            {
                var newButterfly = Instantiate(Resources.Load<ButterflyRenderer>("Animals/Butterfly"));
                butterflies[i] = newButterfly;

                Vector2 butterflyPos = (Vector2)transform.position + RandomNum.GetVector(-1, 1, Target.RandomNumberGenerator);
                butterflyPos.y++;
                newButterfly.Spawn(butterflyPos, i + flower.Seed, flower.butterflySpecies);
                newButterfly.AutoDestroy = false;
            }
        }

        protected override void Destroyed()
        {
            if (butterflies == null)
                return;
            for (int i = 0; i < butterflies.Length; i++)
            {
                if (!butterflies[i].IsDestroyed)
                    Destroy(butterflies[i].gameObject);
            }
        }
    }
}
