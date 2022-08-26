using UnityEngine;

namespace Universe
{
    public class Orbiter : MonoBehaviour
    {
        private float OrbitalRadius;
        private float Rotation;

        [SerializeField]
        private Vector2 centerOfRotation;

        private bool Activated;

        private float _UnitsPerSecond;

        public float UnitsPerSecond
        {
            get => _UnitsPerSecond;
            set
            {
                _UnitsPerSecond = value;

                float orbitalCircumference = Mathf.PI * OrbitalRadius * OrbitalRadius;
                //360 * speed = orbitalCircumference * rotationDelta

                rotationDelta = 360 * _UnitsPerSecond / orbitalCircumference;
            }
        }

        public float Age => GlobalTime.TotalAge / 360f;

        private float rotationDelta;
        private float initialRotation;

        public void Activate(float initialRotation, float rotationRadius, float speed)
        {
            OrbitalRadius = rotationRadius;
            UnitsPerSecond = speed;

            this.initialRotation = initialRotation;
            Rotation = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;

            Activated = true;
        }

        private void Update()
        {
            if (!Activated)
                return;
            if (transform.position == Vector3.zero)
                return;

            float RotationRad = Rotation * Mathf.Deg2Rad;

            transform.position = new Vector2(Mathf.Cos(RotationRad), Mathf.Sin(RotationRad)) * OrbitalRadius + centerOfRotation;

            Rotation = GlobalTime.Time * rotationDelta + initialRotation;

            transform.rotation = Quaternion.Euler(0, 0, Rotation);
        }
    }
}
