using UnityEngine;
using System.Collections.Generic;
using System;

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

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override Type PlanetType => typeof(MoltenPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Width, (float)MoltenPlanet.MinScale, (float)MoltenPlanet.MaxScale) * Vector2.one;

            GenerateLines();
        }

        private void GenerateLines()
        {
            lines = GetLines();

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

        private List<(Vector2, Vector2)> GetLines()
        {
            //Start from the middle and draw 5 lines from the center to random points
            //from each of those lines, draw 2 lines originating from any point on the line and ending at the middle

            const int BaseLines = 4;
            List<(Vector2, Vector2)> lines = new List<(Vector2, Vector2)>();

            for (int i = 0; i < BaseLines; i++)
            {
                float rotation = i * (360f / BaseLines) + RandomNum.GetFloat(5, Target.RandomNumberGenerator);

                //This was a bug, Mathf.Rad2Deg should be Mathf.Deg2Rad but doing that makes it look too normal, I prefer the chaotic sprawl of the bug
                float rotationRad = rotation * Mathf.Rad2Deg;

                Vector2 end = new Vector2(Mathf.Cos(rotationRad), Mathf.Sin(rotationRad)) / 2;
                lines.Add((Vector2.zero, end));

                lines.Add(GetBranch(end, rotation + 15));
                lines.Add(GetBranch(end, rotation - 15));
            }

            return lines;
        }

        private (Vector2, Vector2) GetBranch(Vector2 parentBranchEnd, float rotation)
        {
            float DistanceFromOrigin = RandomNum.GetFloat(1, Target.RandomNumberGenerator);
            Vector2 startPos = Vector2.Lerp(Vector2.zero, parentBranchEnd, DistanceFromOrigin);

            Vector2 direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
            Vector2 endPos = startPos + direction;

            endPos = endPos.normalized / 2;
            return (startPos, endPos);
        }
    }
}
