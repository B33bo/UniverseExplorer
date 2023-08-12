using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets.Iron
{
    public class ScrapRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private GameObject sheet, pole, disc, lights;

        [SerializeField]
        private SpriteRenderer[] lightSprites;
        private Color[] colors;
        private float[] period;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Scrap scrap = new Scrap();
            Target = scrap;
            Scale = Vector2.one;

            if (seed.HasValue)
                scrap.SetSeed(seed.Value);

            scrap.Create(pos);
            scrap.OnInspected += RefreshScrap;

            sheet.transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(6, Target.RandomNumberGenerator) - 3);
            pole.transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(10, 15, Target.RandomNumberGenerator));
            disc.transform.localScale += RandomNum.GetFloat(-.3f, .3f, Target.RandomNumberGenerator) * Vector3.one;
            disc.transform.localPosition += new Vector3(RandomNum.GetFloat(2f, Target.RandomNumberGenerator) - 1, RandomNum.GetFloat(2f, Target.RandomNumberGenerator) - 1);

            colors = new Color[lightSprites.Length];
            period = new float[lightSprites.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.HSVToRGB(RandomNum.GetFloat(1f, Target.RandomNumberGenerator), 1, 1);
                period[i] = 1 / RandomNum.GetFloat(.2f, 3f, Target.RandomNumberGenerator);
            }

            RefreshScrap(null);
        }

        public override void OnUpdate()
        {
            float time = GlobalTime.Time;

            for (int i = 0; i < lightSprites.Length; i++)
            {
                Color c = colors[i];
                c.a = Mathf.Sin(time * period[i]);
                lightSprites[i].color = c;
            }
        }

        private void RefreshScrap(Variable v)
        {
            Scrap scrap = Target as Scrap;
            sheet.SetActive(scrap.Sheet);
            pole.SetActive(scrap.Pole);
            disc.SetActive(scrap.Disc);
            lights.SetActive(scrap.Lights);
        }
    }
}
