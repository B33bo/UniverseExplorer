using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class FireworkRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ParticleSystem particles;

        [SerializeField]
        private CircleCollider2D collision;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Firework firework = new Firework();

            if (seed.HasValue)
                firework.SetSeed(seed.Value);

            firework.Create(pos);
            InitFirework(firework);
        }

        public void InitFirework(Firework firework)
        {
            Target = firework;

            collision.enabled = false;
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(firework.ExplosionRadius);
            main.simulationSpeed = firework.ExplosionSpeed;

            var burst = particles.emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(firework.ParticleCount);

            var gradient = new ParticleSystem.MinMaxGradient(firework.colors);
            gradient.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = gradient;
            collision.radius = firework.ExplosionRadius * .25f;

            StartCoroutine(DetonateWithoutDestroying());
        }

        public IEnumerator Detonate()
        {
            yield return DetonateWithoutDestroying();
            Destroy(gameObject);
        }

        public IEnumerator DetonateWithoutDestroying()
        {
            particles.Play();
            while (particles.isPlaying)
                yield return new WaitForEndOfFrame();
        }
    }
}
