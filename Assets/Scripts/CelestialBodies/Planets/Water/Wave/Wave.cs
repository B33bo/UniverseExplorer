using UnityEngine;

namespace Universe
{
    public class Wave : TerrainObject
    {
        private float offset;

        private void Start()
        {
            scale = 2;
            position = transform.position;

            TerrainObjectReload(CameraControl.Instance.CameraBounds);
        }

        //Overrides base.OnDestroy
        private void OnDestroy() { }

        private void Update()
        {
            offset = Mathf.Sin(GlobalTime.Time + position.x);
            TerrainObjectReload(CameraControl.Instance.CameraBounds);
        }

        protected override void TerrainObjectReload(Rect rect)
        {
            float y = (position.y - rect.yMin) + (scale / 2) + 1;
            transform.localScale = new Vector3(transform.localScale.x, y);
            transform.position = new Vector3(position.x, position.y - y / 2 + scale / 2 + offset);
        }
    }
}
