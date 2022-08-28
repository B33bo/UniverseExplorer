using UnityEngine;

namespace Universe
{
    public class TerrainObject : CelestialBodyRenderer
    {
        protected float scale;
        protected Vector2 position;

        [SerializeField]
        protected float offset;

        private void Start()
        {
            scale = transform.localScale.y;
            position = transform.position;

            CameraControl.Instance.OnPositionUpdate += TerrainObjectReload;
            TerrainObjectReload(CameraControl.Instance.CameraBounds);
            Spawn(transform.position, null);
        }

        //override base.Update
        private void Update() { }

        protected virtual void TerrainObjectReload(Rect rect)
        {
            float y = (position.y - rect.yMin) + (scale / 2) + 1 + offset;
            transform.localScale = new Vector3(transform.localScale.x, y);
            transform.position = new Vector3(position.x, position.y - y / 2 + scale / 2);
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= TerrainObjectReload;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new CelestialBodies.UnknownItem();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }
    }
}
