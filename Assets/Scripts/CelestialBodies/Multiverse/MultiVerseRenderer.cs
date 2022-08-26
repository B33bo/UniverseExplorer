using UnityEngine;

namespace Universe.CelestialBodies
{
    public class MultiVerseRenderer : CelestialBodyRenderer
    {
        private float rotation;

        private void Start()
        {
            if (BodyManager.Parent is null)
                Spawn(Vector2.zero, 0);
        }

        public override void Spawn(Vector2 pos, int? Seed)
        {
            Target = new MultiVerse
            {
                Name = "The Multiverse"
            };
            if (Seed.HasValue)
                Target.Seed = Seed.Value;
            Target.Create(pos);
        }

        public override void OnUpdate()
        {
            rotation += Time.deltaTime * 500;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
