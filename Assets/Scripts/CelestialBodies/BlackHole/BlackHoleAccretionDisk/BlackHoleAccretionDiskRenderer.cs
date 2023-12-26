using UnityEngine;

namespace Universe.CelestialBodies
{
    public class BlackHoleAccretionDiskRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Collider2D collision;

        private void Start()
        {
            LowResScale = 25;
        }

        protected override void LowRes()
        {
            if (BodyManager.Parent is BlackHoleAccretionDisk)
                return;
            collision.enabled = false;
        }

        protected override void HighRes()
        {
            if (BodyManager.Parent is BlackHoleAccretionDisk)
                return;
            collision.enabled = true;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new BlackHoleAccretionDisk();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
            name = Target.Name;
        }

        public void Spawn(Vector2 pos, int? seed, BlackHole blackHole)
        {
            Spawn(pos, seed);
            (Target as BlackHoleAccretionDisk).blackHole = blackHole;
        }
    }
}
