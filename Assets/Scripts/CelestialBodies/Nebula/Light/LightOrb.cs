using UnityEngine;

namespace Universe.CelestialBodies
{
    public class LightOrb : CelestialBody
    {
        public override string TypeString => "Light";

        public override string TravelTarget => "";

        public override bool Circular => true;

        public Color color;
        public float distance, speed, alphaShift, radius;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            radius = RandomNum.GetFloat(0.01f, 5, RandomNumberGenerator);
            Radius = radius * Measurement.mm;
            Mass = 0;
            Name = "Light";

            distance = RandomNum.GetFloat(0f, 2f, RandomNumberGenerator);
            speed = RandomNum.GetFloat(0.1f, 1f, RandomNumberGenerator);
            alphaShift = RandomNum.GetFloat(.01f, 1f, RandomNumberGenerator);

            color = LightColorSpawner.Instance.GetColor(RandomNumberGenerator);
        }

        private void InitNebula(Nebula nebula)
        {
            color = nebula.Bands[RandomNum.Get(0, nebula.Bands.Length, RandomNumberGenerator)].color;

            if (color == Color.black)
            {
                color = RandomNum.GetColor(RandomNumberGenerator);
                return;
            }

            color.r += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
            color.g += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
            color.b += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
        }

        private void InitAurora(Aurora aurora)
        {
            if (aurora.AuroraCol == Aurora.AuroraColor.Rainbow)
            {
                color = new ColorHSV(RandomNum.GetFloat(1f, RandomNumberGenerator), 1, 1);
                return;
            }

            color = aurora.Color;
            color.r += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
            color.g += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
            color.b += RandomNum.GetFloat(-.1f, .1f, RandomNumberGenerator);
        }
    }
}
