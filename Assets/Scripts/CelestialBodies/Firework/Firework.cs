using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class Firework : CelestialBody
    {
        public bool Instant;

        public override string TypeString => "Firework";

        public override string TravelTarget => "Lights";

        public override bool Circular => true;

        [InspectableVar("Explosion Radius")]
        public float ExplosionRadius;

        [InspectableVar("Velocity")]
        public float ExplosionSpeed;

        [InspectableVar("Particles")]
        public int ParticleCount;

        public Gradient colors;

        [InspectableVar("Color A")]
        public Color ColorA
        {
            get => colors.Evaluate(0);
            set
            {
                var colorKeys = colors.colorKeys;
                var alphaKeys = colors.alphaKeys;
                colorKeys[0] = new GradientColorKey(value, 0);
                colors.SetKeys(colorKeys, alphaKeys);
            }
        }

        [InspectableVar("Color B")]
        public Color ColorB
        {
            get => colors.Evaluate(0);
            set
            {
                var colorKeys = colors.colorKeys;
                var alphaKeys = colors.alphaKeys;
                colorKeys[colorKeys.Length - 1] = new GradientColorKey(value, 1);
                colors.SetKeys(colorKeys, alphaKeys);
            }
        }

        [InspectableVar("Rainbow")]
        public bool Rainbow
        {
            get => colors.colorKeys.Length > 2;
            set
            {
                if (value)
                {
                    colors = Resources.Load<GradientContainer>("RainbowGradient").gradient;
                    return;
                }

                colors = new Gradient()
                {
                    colorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0), new GradientColorKey(Color.blue, 1) },
                    alphaKeys = colors.alphaKeys,
                };
            }
        }

        public override void Create(Vector2 pos)
        {
            Position = pos;
            ExplosionRadius = RandomNum.GetFloat(1.5f, 7, RandomNumberGenerator);
            ExplosionSpeed = RandomNum.GetFloat(.3f, 2, RandomNumberGenerator);
            ParticleCount = RandomNum.Get(6, 90, RandomNumberGenerator);

            bool isRainbow = RandomNum.GetBool(20, RandomNumberGenerator);

            Color colorA = new ColorHSV(RandomNum.GetFloat(1, RandomNumberGenerator), 1, 1);
            Color colorB = new ColorHSV(RandomNum.GetFloat(1, RandomNumberGenerator), 1, 1);

            if (isRainbow)
            {
                colors = Resources.Load<GradientContainer>("RainbowGradient").gradient;
            }
            else
            {
                colors = new Gradient()
                {
                    colorKeys = new GradientColorKey[] { new GradientColorKey(colorA, 0), new GradientColorKey(colorB, 1) },
                    alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) },
                    mode = GradientMode.Blend,
                };
            }

            Radius = ExplosionRadius * Measurement.M;
        }
    }
}
