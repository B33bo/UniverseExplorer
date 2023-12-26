using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe.CelestialBodies
{
    public class CloudRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        private float time = 0;
        private bool fadingIn;
        private bool foundInitYet = false;
        private static Color cloudColor = Color.white;
        private static int cloudColorSeed;

        [SerializeField]
        private Texture2D[] cloudTextures;

        public override void OnUpdate()
        {
            time += Time.deltaTime;
            float alpha = fadingIn ? time : 1 - time;
            alpha = Mathf.Clamp01(alpha);

            Color c = cloudColor;
            c.a = alpha;
            meshRenderer.material.color = c;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Cloud cloud = new();
            Target = cloud;

            if (seed.HasValue)
                cloud.SetSeed(seed.Value);

            cloud.Create(pos);
            InitMesh(cloud);

            if (BodyManager.GetSeed() != cloudColorSeed && BodyManager.Parent is Planet p)
            {
                cloudColorSeed = BodyManager.GetSeed();
                ColorHSV color = (ColorHSV)p.waterColor;
                color.s *= .2f;
                cloudColor = color;
            }

            meshRenderer.material.mainTexture = cloudTextures[RandomNum.Get(0, cloudTextures.Length, Target.RandomNumberGenerator)];
        }

        private void InitMesh(Cloud cloud)
        {
            Vector3[] verts = cloud.points;
            Vector2[] UVs = new Vector2[verts.Length];
            int[] tris = new int[3 * (verts.Length - 2)];
            int endPoint = (verts.Length - 1) / 2;

            for (int i = 0; i < verts.Length; i++)
            {
                if (verts[i].y > 0)
                    UVs[i] = new Vector2(verts[i].x + .5f, 1);
                else if (verts[i].y < 0)
                    UVs[i] = new Vector2(verts[i].x + .5f, 0);
                else if (verts[i].y == 0)
                    UVs[i] = new Vector2(verts[i].x + .5f, .5f);

                if (i >= endPoint)
                    continue;

                tris[6 * i] = i;
                tris[6 * i + 1] = i + 1;
                tris[6 * i + 2] = verts.Length - i - 1;

                tris[6 * i + 3] = verts.Length - i - 1;
                tris[6 * i + 4] = i + 1;
                tris[6 * i + 5] = verts.Length - i - 2;
            }

            Mesh mesh = new()
            {
                name = "Cloud",
                vertices = verts,
                uv = UVs,
                triangles = tris,
            };

            meshFilter.mesh = mesh;
        }

        public void Dissapear()
        {
            fadingIn = false;

            if (!foundInitYet)
            {
                foundInitYet = true;
                time = 1;
                return;
            }

            time = 0;
        }

        public void Appear()
        {
            fadingIn = true;

            if (!foundInitYet)
            {
                foundInitYet = true;
                time = 1;
                return;
            }

            time = 0;
        }
    }
}
