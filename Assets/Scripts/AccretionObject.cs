using UnityEngine;

namespace Universe
{
    [System.Serializable]
    public class AccretionObject
    {
        private Transform transform;
        public SpriteRenderer spriteRenderer;
        public float Offset;
        private float OffsetPi;

        [HideInInspector]
        public float RotationOffset;

        public void Init()
        {
            transform = spriteRenderer.transform;
            OffsetPi = 2 * Offset * Mathf.PI;
        }

        public void SetPos(float T, Vector2 ElipseScale, bool Clockwise)
        {
            float newT = T + OffsetPi;
            if (Clockwise)
                newT = -newT;

            float newRot = (T * 40) + RotationOffset;
            Vector2 newPosition = new Vector2(
                Mathf.Cos(newT) / ElipseScale.x,
                Mathf.Sin(newT) / ElipseScale.y);

            if (Clockwise)
                spriteRenderer.sortingOrder = -newT % (Mathf.PI * 2) < Mathf.PI ? 1 : -1;
            else
                spriteRenderer.sortingOrder = newT % (Mathf.PI * 2) > Mathf.PI ? 1 : -1;

            transform.localPosition = newPosition;
            transform.rotation = Quaternion.Euler(0, 0, newRot);
        }
    }
}
