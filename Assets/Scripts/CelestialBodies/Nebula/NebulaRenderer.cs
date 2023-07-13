using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Universe.CelestialBodies
{
    public class NebulaRenderer : CelestialBodyRenderer
    {
        private Nebula nebula;

        [SerializeField]
        private Sprite glowOrb;

        private List<SpriteRenderer> rainbow;

        private float rainbowH;

        public override void OnUpdate()
        {
            if (rainbow.Count == 0)
                return;

            rainbowH = GlobalTime.Time % 1;
            Color c = Color.HSVToRGB(rainbowH, 1, 1);

            for (int i = 0; i < rainbow.Count; i++)
            {
                rainbow[i].color = c;
            }
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            rainbow = new List<SpriteRenderer>();
            nebula = new Nebula();
            Target = nebula;

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);

            for (int i = 0; i < nebula.Bands.Length; i++)
            {
                DrawBand(nebula.Bands[i]);
            }
        }

        private void DrawBand(Nebula.Band band, bool forceRainbow = false)
        {
            var parent = new GameObject(band.color.ToString());
            parent.transform.parent = transform;
            parent.transform.position = transform.position;

            for (int i = 0; i < band.lights.Length; i++)
            {
                var newGlowOrb = new GameObject("orb" + i, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

                if (forceRainbow)
                    rainbow.Add(newGlowOrb);

                newGlowOrb.sprite = glowOrb;
                newGlowOrb.transform.parent = parent.transform;
                newGlowOrb.transform.localPosition = band.lights[i].position;
                newGlowOrb.transform.localScale = band.lights[i].radius * 2 * Vector3.one;

                newGlowOrb.color = band.color;
                newGlowOrb.sortingLayerName = "CelestialBody";
                newGlowOrb.sortingOrder = RandomNum.Get(Target.RandomNumberGenerator);

                if (band.color == Color.black)
                    rainbow.Add(newGlowOrb);

                var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                circleCollider.offset = band.lights[i].position;
                circleCollider.radius = band.lights[i].radius / 2;
            }
        }
    }
}
