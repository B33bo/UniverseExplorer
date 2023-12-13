using UnityEngine;

namespace Universe.Terrain
{
    public class BiomeTemperatureType : MonoBehaviour
    {
        public PolyTerrainLayer Sea, Beach;

        public PolyTerrainLayer[] Normal;
        public float[] Weights;
        public bool UseBeach = true;
        public bool UseSea = true;
    }
}
