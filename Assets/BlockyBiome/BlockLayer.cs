using UnityEngine;

namespace Universe
{
    [CreateAssetMenu(fileName = "BlockBiome")]
    public class BlockLayer : ScriptableObject
    {
        public float Y;
        public float Error;

        public Block block;

        public bool renderTop;
        public Block top;
    }
}
