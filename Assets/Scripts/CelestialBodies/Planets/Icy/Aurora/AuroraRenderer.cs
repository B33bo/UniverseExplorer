using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Universe.CelestialBodies.Planets.Icy
{
    public class AuroraRenderer : CelestialBodyRenderer
    {
        private Aurora aurora;

        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private BoxCollider2D boxCollider;

        [SerializeField]
        private Light2D auroraLight;

        private Mesh mesh;

        private const float speed = .25f;

        private bool renderSine;
        private int halfwaypoint;

        public override void Spawn(Vector2 pos, int? seed)
        {
            pos.y += 20;
            aurora = new Aurora();
            Target = aurora;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            meshFilter.mesh = ShapeMaker.SubdividedRectangle(new Vector2((float)aurora.Width, 3), 20);
            mesh = meshFilter.mesh;

            boxCollider.size = new Vector2((float)aurora.Width, (float)aurora.Height);

            Bounds b = mesh.bounds;
            b.extents = new Vector3(b.extents.x, (float)aurora.Height * 2 + 1);
            mesh.bounds = b;

            halfwaypoint = mesh.vertexCount / 2;

            Target.OnInspected += _ => ResetColors();

            SetLightShape();

            ResetColors();
        }

        private void SetLightShape()
        {
            // should be an elipse
            const float multiplier = .1f;
            float width = (float)aurora.Width * 2, height = (float)aurora.Height * .5f;

            Vector3[] points = new Vector3[30];

            for (int i = 0; i < points.Length; i++)
            {
                float theta = (float)i / points.Length * 2 * Mathf.PI;

                Vector3 value = new(Mathf.Cos(theta) * width, Mathf.Sin(theta) * height);
                points[i] = value * multiplier;
            }

            auroraLight.SetShapePath(points);
        }

        private void ResetColors()
        {
            Material material = meshRenderer.material;
            material.SetColor("_ColorA", aurora.AuroraColor);
            material.SetColor("_ColorB", aurora.AuroraColor - new ColorHSV(0, .2f, .2f));
            material.SetInt("_Rainbow", aurora.IsRainbow ? 1 : 0);

            auroraLight.color = aurora.AuroraColor;
        }

        public override void OnUpdate()
        {
            if (!renderSine)
                return;

            Vector3[] verts = mesh.vertices;
            float dx = 5 / (halfwaypoint - 1f);

            for (int i = 0; i < halfwaypoint; i++)
            {
                float sin = Mathf.Sin(Time.time * speed + i * dx);
                float height = (float)aurora.Height * .5f;

                verts[i + 1].y = sin + height;
                verts[(verts.Length - i) % verts.Length].y = sin - height;
            }
            mesh.vertices = verts;

            if (aurora.IsRainbow)
                auroraLight.color = Color.HSVToRGB(Time.time % 1, 1, 1);
        }

        protected override void HighRes()
        {
            renderSine = true;
        }

        protected override void LowRes()
        {
            renderSine = false;
        }
    }
}
