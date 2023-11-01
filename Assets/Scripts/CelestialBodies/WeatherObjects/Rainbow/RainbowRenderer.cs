using TMPro.EditorUtilities;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class RainbowRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer sprite;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Rainbow();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
            Target.OnInspected += RefreshRainbow;
            RefreshRainbow();
        }

        private void RefreshRainbow(Inspector.Variable val)
        {
            Material mat = sprite.material;
            Rainbow rainbow = Target as Rainbow;

            switch (val.VariableName)
            {
                default:
                    break;
                case "Radius":
                    Scale = rainbow.physicalRadius * Vector2.one;
                    rainbow.Radius = rainbow.physicalRadius * Measurement.Km;
                    break;
                case "Alpha":
                    mat.SetFloat("_Alpha", rainbow.alpha);
                    break;
                case "Ring Size":
                    mat.SetFloat("_Ring", rainbow.ringSize);
                    break;
            }
        }

        public void RefreshRainbow()
        {
            Material mat = sprite.material;
            Rainbow rainbow = Target as Rainbow;

            mat.SetFloat("_Alpha", rainbow.alpha);
            mat.SetFloat("_Ring", rainbow.ringSize);
            Scale = rainbow.physicalRadius * Vector2.one;
        }

        public void SetIntensity(float intensity)
        {
            (Target as Rainbow).SetIntensity(intensity);
            RefreshRainbow();
        }
    }
}
