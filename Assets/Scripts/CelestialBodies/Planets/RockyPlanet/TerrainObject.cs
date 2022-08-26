using UnityEngine;

namespace Universe
{
    public class TerrainObject : MonoBehaviour
    {
        protected float scale;
        protected Vector2 position;

        private void Start()
        {
            scale = transform.localScale.y;
            position = transform.position;

            CameraControl.Instance.OnPositionUpdate += TerrainObjectReload;
            TerrainObjectReload(CameraControl.Instance.CameraBounds);
        }

        protected virtual void TerrainObjectReload(Rect rect)
        {
            float y = (position.y - rect.yMin) + (scale / 2) + 1;
            transform.localScale = new Vector3(transform.localScale.x, y);
            transform.position = new Vector3(position.x, position.y - y / 2 + scale / 2);
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= TerrainObjectReload;
        }
    }
}
