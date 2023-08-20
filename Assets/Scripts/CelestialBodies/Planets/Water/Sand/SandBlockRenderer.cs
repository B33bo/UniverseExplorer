using UnityEngine;

namespace Universe.Blocks
{
    public class SandBlockRenderer : BasicBlockRenderer
    {
        [SerializeField]
        private BoxCollider2D collision;

        [SerializeField]
        private CelestialBodyRenderer[] bodies;

        [SerializeField]
        private float[] weights;

        private CelestialBodyRenderer spawned;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            GenerateItem();
            bodies = null;
            weights = null;
        }

        private void GenerateItem()
        {
            var prefab = bodies[RandomNum.GetIndexFromWeights(weights, Target.RandomNumberGenerator)];

            if (prefab is null)
                return;
            collision.enabled = false;

            spawned = Instantiate(prefab, Target.Position, Quaternion.identity);
            spawned.Spawn(Target.Position, Target.Seed);
        }

        protected override void Destroyed()
        {
            if (spawned)
                Destroy(spawned.gameObject);
        }
    }
}
