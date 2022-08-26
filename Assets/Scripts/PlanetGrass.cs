using UnityEngine;

namespace Universe
{
    public class PlanetGrass : MonoBehaviour
    {
        [SerializeField]
        private float YOffset;

        [SerializeField]
        private bool changeYposAndSize = true;
        private void Update()
        {
            float height = CameraControl.Instance.CameraBounds.height;

            Vector2 position = transform.position;
            position.x = CameraControl.Instance.Position.x;

            if (changeYposAndSize)
            {
                position.y = -height / 2 + YOffset;
                transform.localScale = new Vector3(CameraControl.Instance.CameraBounds.width, height);
            }
            else
                transform.localScale = new Vector3(CameraControl.Instance.CameraBounds.width, transform.localScale.y);
            transform.position = position;
        }
    }
}
