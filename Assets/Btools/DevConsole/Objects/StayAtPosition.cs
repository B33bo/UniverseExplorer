using UnityEngine;

namespace Btools.DevConsole
{
    internal class StayAtPosition : MonoBehaviour
    {
        private Vector3 position;

        private void Start()
        {
            position = transform.position;
        }

        private void Update()
        {
            transform.position = position;
        }
    }
}
