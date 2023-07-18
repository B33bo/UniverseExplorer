using UnityEngine;
using Universe.CelestialBodies;
using System.Collections.Generic;

namespace Universe
{
    public class AuroraRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer auroraFlare;

        private List<SpriteRenderer> flares;

        private Aurora aurora;

        public override void Spawn(Vector2 pos, int? seed)
        {
            pos.y += 5;
            aurora = new Aurora();
            Target = aurora;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
            flares = new List<SpriteRenderer>();
            GetComponent<BoxCollider2D>().size = new Vector3((float)Target.Width, (float)Target.Height);

            float currentPoint = (float)-Target.Width / 2;

            for (int i = 0; i < aurora.Flares; i++)
            {
                var newFlare = Instantiate(auroraFlare, transform);
                newFlare.transform.localPosition = new Vector3(currentPoint, 0);
                flares.Add(newFlare);
                currentPoint += Aurora.FlareSize;
            }
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < flares.Count; i++)
            {
                float time = GlobalTime.Time;

                Vector2 currentPos = flares[i].transform.localPosition;
                currentPos.y = Mathf.Sin(time + (i * Mathf.Deg2Rad));
                flares[i].transform.localPosition = currentPos;

                if (aurora.AuroraCol == Aurora.AuroraColor.Rainbow)
                {
                    float hueValue = Mathf.Sin(time + (i * Mathf.Deg2Rad));
                    hueValue = (hueValue + 1) / 2f;
                    flares[i].color = Color.HSVToRGB(hueValue, 1, 1);
                    continue;
                }

                var color = aurora.Color;
                color.h += Mathf.Sin(time + (i * Mathf.Deg2Rad)) / 20f;
                flares[i].color = color;
            }
        }
    }
}
