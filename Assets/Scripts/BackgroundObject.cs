using UnityEngine;

namespace Universe
{
    public class BackgroundObject : MonoBehaviour
    {
        [SerializeField]
        private float parralax, yParralax;

        [SerializeField]
        private Transform[] backgrounds;

        private int Mod(int a)
        {
            int mod = a % backgrounds.Length;
            if (mod < 0)
                mod += backgrounds.Length;
            return mod;
        }

        private void LateUpdate()
        {
            float scale = 2 * CameraControl.Instance.MyCamera.aspect * CameraControl.Instance.MyCamera.orthographicSize;
            transform.localScale = new Vector3(scale, scale);

            float camX = CameraControl.Instance.Position.x;
            float parralaxOffset = camX * parralax;
            camX += parralaxOffset;

            int index = Mathf.FloorToInt(camX / scale);
            float startPoint = index * scale;

            transform.position = new Vector3(startPoint + scale - parralaxOffset, CameraControl.Instance.Position.y * yParralax);

            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].localPosition = new Vector3(backgrounds.Length - 2 - Mod(index + i), 0);
            }
        }
    }
}
