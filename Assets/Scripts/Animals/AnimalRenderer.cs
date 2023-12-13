using System.Collections.Generic;
using UnityEngine;

namespace Universe.Animals
{
    public abstract class AnimalRenderer : CelestialBodyRenderer
    {
        public static List<int> seedsSpawned = new List<int>();
        public abstract System.Type AnimalType { get; }
        public bool AutoDestroy = true;

        public override void Spawn(Vector2 pos, int? seed)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (AutoDestroy &&
                (transform.position.x < CameraControl.Instance.CameraBounds.xMin - 10 ||
                transform.position.x > CameraControl.Instance.CameraBounds.xMax + 10))
                Destroy(gameObject);
            OnUpdate();
        }

        public abstract void Spawn(Vector2 position, int? seed, Animal species);

        protected override void Destroyed()
        {
            seedsSpawned.Remove(Target.Seed);
        }

        public void SetPattern(Pattern pattern, SpriteMask spriteMask, SpriteRenderer sprite)
        {
            sprite.sortingOrder = pattern.id;
            spriteMask.backSortingOrder = pattern.id - 1;
            spriteMask.frontSortingOrder = pattern.id + 1;

            sprite.sprite = Resources.Load<Sprite>("Patterns/" + pattern.patternType.ToString());
            Material mat = sprite.material;

            mat.SetColor("_ColorA", pattern.Primary);
            mat.SetColor("_ColorB", pattern.Secondary);
            mat.SetColor("_ColorC", pattern.Tertiary);

            sprite.transform.localRotation = Quaternion.Euler(0, 0, pattern.rotation);
        }
    }
}
