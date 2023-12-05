using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class ToxicPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private LineRenderer[] goo;

        public override Type PlanetType => typeof(ToxicPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            ToxicPlanet toxicPlanet = new ToxicPlanet();
            Target = toxicPlanet;

            Scale = (float)GetFairSize(Target.Width, ToxicPlanet.MinScale, ToxicPlanet.MaxScale) * 5 * Vector2.one;

            if (seed.HasValue)
                toxicPlanet.SetSeed(seed.Value);

            toxicPlanet.OnInspected += ColorReset;
            toxicPlanet.Create(pos);

            List<(Vector2, Vector2)> lines = ShapeMaker.GetRandomWebLines(Target.RandomNumberGenerator);

            for (int i = 0; i < lines.Count; i++)
            {
                LineRenderer lineRenderer = goo[i];
                lineRenderer.SetPosition(0, lines[i].Item1);
                lineRenderer.SetPosition(1, lines[i].Item2);
                lineRenderer.startColor = toxicPlanet.ToxicColor;
                lineRenderer.endColor = toxicPlanet.ToxicColor;
            }
        }

        private void ColorReset(Variable v)
        {
            if (v.VariableName == "Type")
                (Target as ToxicPlanet).RefreshColor();
            Color c = (Target as ToxicPlanet).ToxicColor;
            for (int i = 0; i < goo.Length; i++)
            {
                goo[i].startColor = c;
                goo[i].endColor = c;
            }
        }

        protected override void HighRes()
        {
            base.HighRes();
            for (int i = 0; i < goo.Length; i++)
                goo[i].enabled = true;
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            base.LowRes();
            for (int i = 0; i < goo.Length; i++)
                goo[i].enabled = false;
        }
    }
}
