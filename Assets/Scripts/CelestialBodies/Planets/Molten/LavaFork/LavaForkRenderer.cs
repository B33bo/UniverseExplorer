using UnityEngine;

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

                var main = particles.main;
                main.startColor = new ParticleSystem.MinMaxGradient(ColorHighlights.Instance.primary, ColorHighlights.Instance.tertiary);
            }
                
            LoadLines();
            Destroy(line.gameObject);
        }

        private void LoadLines()
        {
            LavaFork lavaFork = Target as LavaFork;
            LoadTree(lavaFork.tree, Vector2.zero);
        }

        private void LoadTree(LavaFork.Node node, Vector2 startPos)
        {
            Vector2 change = new(Mathf.Cos(node.angleOffset * Mathf.Deg2Rad), Mathf.Sin(node.angleOffset * Mathf.Deg2Rad));
            change *= node.length;

            Vector2 endPos = startPos + change;
            LineRenderer newLine = Instantiate(line, transform);
            newLine.SetPosition(0, startPos);
            newLine.SetPosition(1, endPos);
            newLine.startWidth = .4f / (node.depth + 1);
            newLine.endWidth = .4f / (node.depth + 1);

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
