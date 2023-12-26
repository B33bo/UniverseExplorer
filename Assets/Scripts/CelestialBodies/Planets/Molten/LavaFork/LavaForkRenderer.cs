using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Universe.CelestialBodies.Planets.Molten
{
    public class LavaForkRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        protected LineRenderer line;

        [SerializeField]
        private ParticleSystem particles;

        [SerializeField]
        private bool UseColorHighlights;

        [SerializeField]
        protected Light2D lavaLight;

        private float minY = float.PositiveInfinity;
        private float maxX = 0;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new LavaFork();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);

            if (UseColorHighlights)
            {
                line.material.SetColor("_ColorA", ColorHighlights.Instance.primary);
                line.material.SetColor("_ColorB", ColorHighlights.Instance.secondary);
                line.material.SetColor("_ColorC", ColorHighlights.Instance.tertiary);

                if (particles)
                {
                    var main = particles.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(ColorHighlights.Instance.primary, ColorHighlights.Instance.tertiary);
                }

                lavaLight.color = ColorHighlights.Instance.primary;
            }

            LoadLines();
            LoadLightPath();

            Destroy(line.gameObject);
        }

        private void LoadLightPath()
        {
            const float scaleFactor = .2f;
            Vector3 centre = new(0, minY * .5f);
            Vector3 point1 = new(0, 0);
            Vector3 point2 = new(-maxX, minY);
            Vector3 point3 = new(maxX, minY);

            point1 = (point1 - centre) * scaleFactor + centre;
            point2 = (point2 - centre) * scaleFactor + centre;
            point3 = (point3 - centre) * scaleFactor + centre;

            lavaLight.SetShapePath(new Vector3[]
            {
                point1,
                point2,
                point3,
            });
        }

        private void LoadLines()
        {
            LavaFork lavaFork = Target as LavaFork;
            LoadTree(lavaFork.tree, Vector2.zero);
        }

        protected virtual void OnAddPosition(Vector3 pos, LineRenderer line) { }

        private void LoadTree(LavaFork.Node node, Vector2 startPos)
        {
            Vector2 change = new(Mathf.Cos(node.angleOffset * Mathf.Deg2Rad), Mathf.Sin(node.angleOffset * Mathf.Deg2Rad));
            change *= node.length;

            Vector2 endPos = startPos + change;
            LineRenderer newLine = Instantiate(line, transform);
            newLine.SetPosition(0, startPos);
            newLine.SetPosition(1, endPos);
            newLine.startWidth = .4f / (node.depth + 1);
            newLine.endWidth = newLine.startWidth;

            if (endPos.y < minY)
                minY = endPos.y;

            if (Mathf.Abs(endPos.x) > maxX)
                maxX = Mathf.Abs(endPos.x);
            if (Mathf.Abs(startPos.x) > maxX)
                maxX = Mathf.Abs(startPos.x);

            OnAddPosition(endPos, newLine);

            if (node.branches == null || node.branches.Length == 0)
            {
                newLine.numCapVertices = 3;
                return;
            }

            for (int i = 0; i < node.branches.Length; i++)
                LoadTree(node.branches[i], endPos);
        }
    }
}
