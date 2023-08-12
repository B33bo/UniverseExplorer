using UnityEngine;

namespace Universe.CelestialBodies.Planets.Toxic
{
    public class ToxicGasRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ParticleSystem particles;

        public override void Spawn(Vector2 pos, int? seed)
        {
            pos.y += 2;
            Target = new ToxicGas();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            var main = particles.main;
            ColorHSV a = ColorHighlights.Instance.primary;
            ColorHSV b = a;

            a.s -= .1f;
            a.v -= .1f;
            b.s += .1f;
            b.v += .1f;

            main.startColor = new ParticleSystem.MinMaxGradient(a, b);

            var toxicGas = Target as ToxicGas;
            main.startSpeed = toxicGas.Speed;
            main.startLifetime = toxicGas.GasWidth / toxicGas.Speed;
        }
    }
}
