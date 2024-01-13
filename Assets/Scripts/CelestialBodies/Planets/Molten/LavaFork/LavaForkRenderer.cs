using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Universe.CelestialBodies.Planets.Molten
{
    public class LavaForkRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ParticleSystem particles;

        [SerializeField]
        private bool UseColorHighlights;

        [SerializeField]
        protected Light2D lavaLight;

        [SerializeField]
        private MeshFilter meshFilter;
        protected MeshRenderer meshRenderer;

        private float minY = float.PositiveInfinity;
        private float maxX = 0;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new LavaFork();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);

            meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("_Offset", RandomNum.GetFloat(1, Target.RandomNumberGenerator));

            if (UseColorHighlights)
            {
                meshRenderer.material.SetColor("_ColorA", ColorHighlights.Instance.primary);
                meshRenderer.material.SetColor("_ColorB", ColorHighlights.Instance.secondary);
                meshRenderer.material.SetColor("_ColorC", ColorHighlights.Instance.tertiary);

                if (particles)
                {
                    var main = particles.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(ColorHighlights.Instance.primary, ColorHighlights.Instance.tertiary);
                }

                lavaLight.color = ColorHighlights.Instance.primary;
            }

            LoadLines();
            LoadLightPath();
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
            meshFilter.mesh = TreeNodeMeshDrawer.GenerateMesh(lavaFork.tree, TreeNodeMeshDrawer.UVMode.DepthBased);

            (maxX, minY) = lavaFork.tree.GetMaxXMinY();
        }

        protected virtual void OnAddPosition(Vector3 pos, LineRenderer line) { }
    }
}
