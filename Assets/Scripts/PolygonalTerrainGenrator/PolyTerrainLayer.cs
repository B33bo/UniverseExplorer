using UnityEngine;

namespace Universe.Terrain
{
    [System.Serializable]
    public class PolyTerrainLayer
    {
        public TerrainNoise[] Noises;
        public PolyTerrainRenderer Renderer;
        public float Offset;
        public float MinimumHeight;
        public bool OverrideColor;
        public Color color;

        public float HeightAtPoint(float x)
        {
            float noise = 0;
            for (int i = 0; i < Noises.Length; i++)
            {
                noise += Noises[i].NoiseAt(new Vector2(x, Offset + BodyManager.GetSeed()));
            }
            return noise + MinimumHeight;
        }
    }
}