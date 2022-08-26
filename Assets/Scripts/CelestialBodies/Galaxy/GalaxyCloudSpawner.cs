using UnityEngine;

namespace Universe.CelestialBodies
{
    public class GalaxyCloudSpawner : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer Outer, Inner;

        [SerializeField]
        private int textureScale;

        [SerializeField]
        private float scaleFactor;

        private bool IsRainbow = false;

        private void Start()
        {
            if (!(BodyManager.Parent is SpiralGalaxy Galaxy))
            {
                Galaxy = new SpiralGalaxy();
                Galaxy.SetSeed(0);
                Galaxy.Create(Vector2.zero);
                Galaxy.IsRainbow = false;
                Galaxy.Color = (Color.red, Color.blue);
            }

            if (Galaxy.IsRainbow)
            {
                IsRainbow = true;
                return;
            }

            Outer.color = Galaxy.Color.outer;
            Inner.color = Galaxy.Color.inner;
        }

        private void LateUpdate()
        {
            if (IsRainbow)
            {
                Outer.color = Color.HSVToRGB(Time.time % 1, 1, 1);
                Inner.color = Color.HSVToRGB((-Time.time % 1) + 1, 1, 1);
            }

            Vector2 cameraPos = CameraControl.Instance.Position;
            transform.position = cameraPos;

            transform.localScale = CameraControl.Instance.CameraBounds.width * Vector2.one;
        }
    }
}
