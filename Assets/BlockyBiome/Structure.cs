using UnityEngine;

namespace Universe
{
    public abstract class Structure : MonoBehaviour
    {
        public abstract void Spawn(Vector2 pos, int seed);
    }
}
