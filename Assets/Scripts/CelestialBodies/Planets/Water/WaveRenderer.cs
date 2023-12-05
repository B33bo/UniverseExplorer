using Btools.DevConsole;
using UnityEngine;
using Universe.Terrain;

namespace Universe
{
    public class WaveRenderer : PolyTerrainRenderer
    {
        private static readonly float[] NumCache = new float[] { 1, 2, 3, 1, 4, 1.2f, 4.5f, 6f, 3.1f }; // random nums 
        private static float[] waveLengths;
        private static float[] waveAmplitudes;
        private static bool preLoadedWaveLengths = false;
        private static float waveFrequency = .1f;
        private static float height = .3f;
        private static float speed = .4f;
        private static int waves = 5;

        private Mesh mesh;
        private float originalPos;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);
            mesh = meshFilter.mesh;
            originalPos = mesh.vertices[0].y;

            if (!preLoadedWaveLengths)
                PreloadWaves();
        }

        public override void Spawn(Vector2 pos, int? seed, PolyTerrain previous, PolyTerrainLayer layer)
        {
            base.Spawn(pos, seed, previous, layer);
            mesh = meshFilter.mesh;
            originalPos = mesh.vertices[0].y;

            if (!preLoadedWaveLengths)
                PreloadWaves();
        }

        public override void OnUpdate()
        {
            Vector3[] verts = mesh.vertices;
            int length = (int)(verts.Length * .5f);

            for (int i = 0; i < length; i++)
            {
                verts[i].y = SumOfSines(Target.Position.x + verts[i].x) + originalPos;
            }
            mesh.vertices = verts;
        }

        private float Sin(float x, float waveLength, float peak, float time)
        {
            return Mathf.Sin(waveFrequency * x * waveLength + time) * peak;
        }

        private float SumOfSines(float x)
        {
            float val = 0;

            for (int i = 0; i < waves; i++)
                val += Sin(x, waveLengths[i], waveAmplitudes[i] * height, GlobalTime.Time * speed);
            return val;
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

            DevCommands.RegisterVar(new DevConsoleVariable("WaveSines", "Sines of waves", typeof(int), () => waves, x => { waves = int.Parse(x); PreloadWaves(); }));
            DevCommands.RegisterVar(new DevConsoleVariable("WaveHeight", "Height of waves", typeof(float), () => height, x => { height = float.Parse(x); }));
            DevCommands.RegisterVar(new DevConsoleVariable("WaveFrequency", "Frequency of waves", typeof(float), () => waveFrequency, x => { waveFrequency = float.Parse(x); }));
            DevCommands.RegisterVar(new DevConsoleVariable("WaveSpeed", "Speed of waves", typeof(float), () => speed, x => { speed = float.Parse(x); }));
        }
    }
}
