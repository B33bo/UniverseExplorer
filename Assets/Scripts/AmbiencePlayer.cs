using UnityEngine;

namespace Universe
{
    public class AmbiencePlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;

        [SerializeField]
        private float distanceImpact = .2f;

        [SerializeField]
        private float dist;

        private void Start()
        {
            CameraControl.Instance.OnPositionUpdate += RefreshSound;
            RefreshSound(CameraControl.Instance.CameraBounds);
        }

        private void RefreshSound(Rect bounds)
        {
            source.volume = GetDistanceValueToCamera(bounds);
        }

        private float GetDistanceValueToCamera(Rect bounds)
        {
            float height = bounds.height;
            Vector3 position = new Vector3(bounds.center.x, bounds.center.y, height);
            float distanceToPosition = (position - transform.position).sqrMagnitude;

            dist = (distanceImpact / distanceToPosition);
            return dist;
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= RefreshSound;
        }
    }
}
