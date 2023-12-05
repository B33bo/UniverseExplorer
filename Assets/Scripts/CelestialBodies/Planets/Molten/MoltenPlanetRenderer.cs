using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe.CelestialBodies.Planets
{
    public class MoltenPlanetRenderer : PlanetRenderer
    {
        private List<(Vector2, Vector2)> lines;

        [SerializeField]
        private LineRenderer[] lineRenderers;

        private bool LerpDirection;
        private float LerpAmount;

        [SerializeField]
        private Color lerpStart, lerpEnd;

        public override Type PlanetType => typeof(MoltenPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Width, (float)MoltenPlanet.MinScale, (float)MoltenPlanet.MaxScale) * Vector2.one;

            GenerateLines();
        }

        private void GenerateLines()
        {
            lines = ShapeMaker.GetRandomWebLines(Target.RandomNumberGenerator);

            for (int i = 0; i < lines.Count; i++)
            {
                LineRenderer lineRenderer = lineRenderers[i];
                lineRenderer.SetPosition(0, lines[i].Item1);
                lineRenderer.SetPosition(1, lines[i].Item2);
            }
        }

        public override void OnUpdate()
        {
            int BackgroundDirectionInt = LerpDirection ? 1 : -1;

            LerpAmount += Time.deltaTime * BackgroundDirectionInt;

            if (LerpAmount > 1 || LerpAmount < 0)
                LerpDirection = !LerpDirection;

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                Color newColor = Color.Lerp(lerpStart, lerpEnd, LerpAmount);
                lineRenderers[i].startColor = newColor;
                lineRenderers[i].endColor = newColor;
            }
        }

        protected override void HighRes()
        {
            base.HighRes();
            for (int i = 0; i < lineRenderers.Length; i++)
                lineRenderers[i].enabled = true;
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            base.LowRes();
            for (int i = 0; i < lineRenderers.Length; i++)
                lineRenderers[i].enabled = false;
        }
    }
}
