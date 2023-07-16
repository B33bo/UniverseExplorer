using UnityEngine;

namespace Universe.CelestialBodies
{
    public class LightOrbRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer, glow;

        [SerializeField]
        private CircleCollider2D collision;

        private LightOrb lightOrb;
        private float orbitalRotation;
        private Vector3 origin;

        public override void Spawn(Vector2 pos, int? seed)
        {
            lightOrb = new LightOrb();
            Target = lightOrb;

            if (seed.HasValue)
                lightOrb.SetSeed(seed.Value);

            origin = pos;
            lightOrb.Create(pos);
            spriteRenderer.color = lightOrb.color;
            transform.localScale = lightOrb.radius * Vector2.one;
        }

        public override void OnUpdate()
        {
            orbitalRotation = (GlobalTime.Time * lightOrb.speed) % 2 * Mathf.PI;
            transform.position = new Vector3(Mathf.Cos(orbitalRotation), Mathf.Sin(orbitalRotation)) * lightOrb.distance;
            transform.position += origin;

            float alpha = Mathf.Sin(GlobalTime.Time * lightOrb.alphaShift);
            Color col = spriteRenderer.color;
            col.a = alpha;
            spriteRenderer.color = col;
            glow.color = spriteRenderer.color;

            collision.enabled = alpha > 0;
        }
    }
}
