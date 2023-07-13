using UnityEngine;

namespace Universe
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float speed;

        private void FixedUpdate()
        {
            Vector3 pos = transform.position;
            Vector3 newPos = Vector2.Lerp(pos, target.position, Time.deltaTime * speed);

            newPos.z = pos.z;
            transform.position = newPos;
        }
    }
}
