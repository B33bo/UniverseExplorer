using Btools.DevConsole;
using UnityEngine;

namespace Universe.Blocks
{
    public class WaveRenderer : CelestialBodyRenderer
    {
        private static readonly float[] NumCache = new float[] { 1, 2, 3, 5, 4, 1.2f, 4.5f, 6f, 3.1f }; // random nums 
        private const float speed = .2f;
        private const float waveHeight = .2f;
        private const float waveScale = .2f;
        private const float height = 20;
        private static int _res = 30;
        private static int Res { get => _res; set { _res = value; OnResetMesh(); } }

        private static bool preLoadedWaveLengths = false;
        private static float[] waveLengths;
        private static float[] waveAmplitudes;
        private static int waves = 10;
        private delegate void ResetMesh();
        private static event ResetMesh OnResetMesh;

        private Mesh mesh;

        public override void Spawn(Vector2 pos, int? seed)
        {
            pos.y = .5f;
            Wave wave = new Wave();
            Target = wave;

            if (seed.HasValue)
                wave.SetSeed(seed.Value);
            wave.Create(pos);

            ResetCurrMesh();
            OnResetMesh += ResetCurrMesh;

            if (preLoadedWaveLengths)
                return;

            PreloadWaves();
        }

        protected override void Destroyed()
        {
            OnResetMesh -= ResetCurrMesh;
        }

        private void ResetCurrMesh()
        {
            mesh = GetWaveMesh(Res);
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private Mesh GetWaveMesh(int res)
        {
            Vector3[] verts = new Vector3[res * 2];

            for (int i = 0; i < res; i++)
            {
                float x = i / (res - 1f) - .5f;
                verts[verts.Length - i - 1] = new Vector2(x, 0);
                verts[i] = new Vector2(x, height * 2); // height * 2 because otherwse it pops out of existence ???
            }

            int[] tris = new int[(res - 1) * 6];

            for (int i = 0; i < res - 1; i++)
            {
                int triStartIndex = i * 6;
                tris[triStartIndex] = verts.Length - 1 - i;
                tris[triStartIndex + 1] = i;
                tris[triStartIndex + 2] = verts.Length - 2 - i;

                tris[triStartIndex + 3] = i;
                tris[triStartIndex + 4] = i + 1;
                tris[triStartIndex + 5] = verts.Length - 2 - i;
            }

            return new Mesh()
            {
                name = "Waterrrr",
                vertices = verts,
                triangles = tris,
            };
        }

        private static void PreloadWaves()
        {
            preLoadedWaveLengths = true;
            waveLengths = new float[waves];
            waveAmplitudes = new float[waves];

            for (int i = 0; i < waves; i++)
            {
                waveLengths[i] = NumCache[i % NumCache.Length] * Mathf.Pow(1.23f, i);
                waveAmplitudes[i] = NumCache[(i + 3) % NumCache.Length] * Mathf.Pow(.8f, i); // brownain something
            }

            DevCommands.RegisterVar(new DevConsoleVariable("WaveRes", "Resolution of waves", typeof(int), () => Res, x => Res = int.Parse(x)));
            DevCommands.RegisterVar(new DevConsoleVariable("WaveSines", "Sines of waves", typeof(int), () => waves, x => { waves = int.Parse(x); PreloadWaves(); }));
        }

        private float Sin(float x, float waveLength, float peak, float time)
        {
            return Mathf.Sin(waveScale * x * waveLength + time) * peak;
        }

        private float SumOfSines(float x)
        {
            float val = 0;

            for (int i = 0; i < waves; i++)
                val += Sin(x, waveLengths[i], waveAmplitudes[i], GlobalTime.Time * speed);
            return val;
        }

        public override void OnUpdate()
        {
            Vector3[] verticies = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                verticies[i] = mesh.vertices[i];

                if (i < Res)
                    verticies[i].y = SumOfSines(Target.Position.x + verticies[i].x) * waveHeight + height;
            }

            mesh.vertices = verticies;
        }
    }
}
