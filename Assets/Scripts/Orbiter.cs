using UnityEngine;

namespace Universe
{
    public class Orbiter : MonoBehaviour
    {
        private float OrbitalRadius;
        private float Rotation;
        private CelestialBody celestialBody;

        public Vector2 centerOfRotation;

        private bool Activated;

        private float _UnitsPerSecond;

        public float UnitsPerSecond
        {
            get => _UnitsPerSecond;
            set
            {
                _UnitsPerSecond = value;

                float orbitalCircumference = Mathf.PI * OrbitalRadius * OrbitalRadius;
                // 360 * speed = orbitalCircumference * rotationDelta

                rotationDelta = 360 * _UnitsPerSecond / orbitalCircumference;
            }
        }

        private float rotationDelta;
        private float initialRotation;

        public void Activate(float initialRotation, float rotationRadius, float speed)
        {
            OrbitalRadius = rotationRadius;
            UnitsPerSecond = speed;

            this.initialRotation = initialRotation;
            Rotation = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;

            Activated = true;

            CelestialBodyRenderer bodyRenderer = GetComponent<CelestialBodyRenderer>();
            if (bodyRenderer is null)
                return;
            celestialBody = bodyRenderer.Target;
        }

        private void Update()
        {
            if (!Activated)
                return;

            Debug.DrawLine(transform.position, centerOfRotation, Color.green);
            float RotationRad = Rotation * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(Mathf.Cos(RotationRad), Mathf.Sin(RotationRad)) * OrbitalRadius;

            if (celestialBody is null)
                transform.position = pos + centerOfRotation;
            else
                celestialBody.Position = pos + centerOfRotation;

            Rotation = GlobalTime.Time * rotationDelta + initialRotation;
            Rotation %= 360;

            transform.rotation = Quaternion.Euler(0, 0, Rotation);
        }
    }
}
