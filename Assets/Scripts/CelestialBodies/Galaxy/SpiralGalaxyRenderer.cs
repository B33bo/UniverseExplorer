using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Universe.CelestialBodies
{
    public class SpiralGalaxyRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private new ParticleSystem particleSystem;

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

            particleSystem.Play();

            StartCoroutine(SkipForwards());
        }

        private void ParticleColorInit() //Particle colour, innit bruv
        {
            if (RandomNum.GetBool(4, Target.RandomNumberGenerator) || Target.Seed == 1337) // 1 / 1,073,741,823 chance (rainbow galaxy)
            {
                (Target as SpiralGalaxy).IsRainbow = true;
                return;
            }

            float color = RandomNum.GetFloat(1f, Target.RandomNumberGenerator);
            Color outerColor = Color.HSVToRGB(color, 1, 1);
            Color innerColor = Color.HSVToRGB(RandomNum.GetFloat(1, Target.RandomNumberGenerator), 1, 1);
            (Target as SpiralGalaxy).Color = (outerColor, innerColor);

            Gradient gradient = new Gradient
            {
                colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(innerColor, .3f), new GradientColorKey(outerColor, 1) },
                alphaKeys = new GradientAlphaKey[] {new GradientAlphaKey(1, 0), new GradientAlphaKey(1, .833f), new GradientAlphaKey(0, 1)}
            };

            var ColorOverLifetime = particleSystem.colorOverLifetime;
            ColorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
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
