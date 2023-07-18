using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class SolarFlareRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ParticleSystem particles;

        public override void Spawn(Vector2 pos, int? seed)
        {
            SolarFlare flare = new SolarFlare();
            Target = flare;

            if (seed.HasValue)
                flare.SetSeed(seed.Value);

            flare.Create(pos);
            var main = particles.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(flare.duration);
            main.startSpeed = new ParticleSystem.MinMaxCurve(5f, flare.speed);

            if (BodyManager.Parent is Star star)
            {
                ColorHSV a = star.StarColor, b = star.StarColor;

                a.h -= .1f;
                b.h += .1f;
                b.h %= 1;

                if (a.h < 0)
                    a.h += 1;

                main.startColor = new ParticleSystem.MinMaxGradient(a, b);
            }
        }
    }
}
