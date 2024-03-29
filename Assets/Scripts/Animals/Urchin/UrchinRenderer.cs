using UnityEngine;

namespace Universe.AnimalsOld
{
    public class UrchinRenderer : AnimalRenderer
    {
        public override System.Type AnimalType => typeof(Urchin);

        [SerializeField]
        private SpriteRenderer pattern;

        [SerializeField]
        private SpriteMask[] masks;

        private Vector2 initPos;

        public override void Spawn(Vector2 position, int? seed, AnimalOld species)
        {
            Urchin urchin = new Urchin();

            if (seed.HasValue)
                urchin.SetSeed(seed.Value);

            urchin.CreateAnimal(species);
            Target = urchin;

            pattern.sortingOrder = urchin.pattern.id;

            for (int i = 0; i < masks.Length; i++)
            {
                masks[i].frontSortingOrder = pattern.sortingOrder + 1;
                masks[i].backSortingOrder = pattern.sortingOrder - 1;
            }

            transform.position = position;

            initPos = position;
            SetPattern(urchin.pattern, masks[0], pattern);

            transform.localScale = urchin.size * Vector2.one;
        }

        public override void OnUpdate()
        {
            transform.SetPositionAndRotation(new Vector3(initPos.x, Mathf.Sin(Time.time * .2f) + initPos.y),
                Quaternion.Lerp(Quaternion.Euler(0, 0, -5), Quaternion.Euler(0, 0, 5), GlobalTime.SinTime));
        }
    }
}
