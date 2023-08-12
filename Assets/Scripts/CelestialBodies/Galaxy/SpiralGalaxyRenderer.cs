using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class SpiralGalaxyRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private new ParticleSystem particleSystem;
        private static Gradient rainbowGradient;

        public override void Spawn(Vector2 position, int? seed)
        {
            Target = new SpiralGalaxy();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(position);

            transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(1, Target.RandomNumberGenerator));

            var scale = GetFairSize((float)Target.Height, (float)SpiralGalaxy.MinWidth, (float)SpiralGalaxy.MaxWidth);

            Scale = scale * Vector2.one;
            particleSystem.transform.localScale = scale / 10f * Vector2.one;
            particleSystem.randomSeed = unchecked((uint)Target.Seed);

            ParticleColorInit();
            (Target as SpiralGalaxy).OnInspected += _ =>
            {
                var TargetGalaxy = (Target as SpiralGalaxy);
                SetColor(TargetGalaxy.outer, TargetGalaxy.inner, TargetGalaxy.IsRainbow);
            };

            particleSystem.Play();

            StartCoroutine(SkipForwards());
        }

        private void ParticleColorInit() //Particle colour, innit bruv
        {
            rainbowGradient ??= particleSystem.colorOverLifetime.color.gradient;
            var target = Target as SpiralGalaxy;

            SetColor(target.outer, target.inner, target.IsRainbow);
        }

        private void SetColor(Color outer, Color inner, bool isRainbow)
        {
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
            Gradient gradient;

            if (isRainbow)
            {
                gradient = rainbowGradient;
            }
            else
            {
                gradient = new Gradient
                {
                    colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(inner, .3f), new GradientColorKey(outer, 1) },
                    alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, .833f), new GradientAlphaKey(0, 1) }
                };
            }

            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        }

        private IEnumerator SkipForwards()
        {
            var x = particleSystem.main;
            x.simulationSpeed = 4000;
            yield return new WaitForSeconds(.2f);
            yield return new WaitForEndOfFrame();
            x.simulationSpeed = 1;
        }

        public override void OnUpdate()
        {
            particleSystem.transform.localScale = transform.localScale / 10;
        }
    }
}
