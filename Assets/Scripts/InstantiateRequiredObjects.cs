using UnityEngine;

namespace Universe
{
    public class InstantiateRequiredObjects : MonoBehaviour
    {
        private static bool Loaded = false;
        [SerializeField]
        private GameObject Camera, Canvas;

        private void Awake()
        {
            if (Loaded)
                return;
            Loaded = true;
            DontDestroyOnLoad(Instantiate(Canvas));
            Instantiate(Camera);
        }
    }
}
