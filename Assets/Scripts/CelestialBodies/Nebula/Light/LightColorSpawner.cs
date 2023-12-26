using UnityEngine;
using Universe.CelestialBodies;

namespace Universe
{
    public class LightColorSpawner : MonoBehaviour
    {
        public static LightColorSpawner Instance { get; private set; }

        [SerializeField]
        private Gradient opalGradient;

        private void Awake()
        {
            Instance = this;
        }

        public Color GetColor(System.Random random)
        {
            if (BodyManager.Parent is Nebula nebula)
                return GetNebula(nebula, random);
            else if (BodyManager.Parent is Aurora aurora)
                return GetAurora(aurora, random);
            else if (BodyManager.Parent is Rainbow r)
                return new ColorHSV(RandomNum.GetFloat(1, random), 1, 1);
            return RandomNum.GetColor(random);
        }

        private Color GetNebula(Nebula nebula, System.Random rand)
        {
            Color color = nebula.Bands[RandomNum.Get(0, nebula.Bands.Length, rand)].color;

            if (color == Color.black)
                return RandomNum.GetColor(rand);

            color.r += RandomNum.GetFloat(-.1f, .1f, rand);
            color.g += RandomNum.GetFloat(-.1f, .1f, rand);
            color.b += RandomNum.GetFloat(-.1f, .1f, rand);
            return color;
        }

        private Color GetAurora(Aurora aurora, System.Random rand)
        {
            if (aurora.IsRainbow)
            {
                return new ColorHSV(RandomNum.GetFloat(1f, rand), 1, 1);
            }

            Color color = aurora.AuroraColor;
            color.r += RandomNum.GetFloat(-.1f, .1f, rand);
            color.g += RandomNum.GetFloat(-.1f, .1f, rand);
            color.b += RandomNum.GetFloat(-.1f, .1f, rand);
            return color;
        }

        private Color GetOpal(System.Random rand)
        {
            return opalGradient.Evaluate((float)rand.NextDouble());
        }
    }
}
