using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe
{
    public abstract class CelestialBodyRenderer : MonoBehaviour
    {
        public CelestialBody Target { get; set; }

        public bool CameraFocus = true;

        public Transform cameraLerpTarget;
        public float cameraLerpSize = 1;
        public bool cameraLerpMultiplyBySize = true;
        public bool scaleLerp = true;
        public bool IsDestroyed;
        public bool IsLowRes { get; private set; }
        public float LowResScale = float.NaN;

        public Vector2 Scale { get; set; }

        private void Awake()
        {
            Scale = transform.localScale;

            if (scaleLerp)
                transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (cameraLerpTarget == null)
                cameraLerpTarget = transform;

            if (Target is null)
            {
                Debug.LogError("Target is null on object " + name);
                Target = new CelestialBodies.UnknownItem();
            }

            ToggleHiddenIfShowing();

            transform.localPosition = Target.Position;
            if (scaleLerp)
            {
                if (IsLowRes)
                {
                    transform.localScale = Scale;
                    scaleLerp = false;
                }
                else
                    transform.localScale = Vector2.Lerp(transform.localScale, Scale, Time.deltaTime * 3);
            }

            name = Target.Name;
            OnUpdate();
        }

        private void ToggleHiddenIfShowing()
        {
            if (LowResScale == float.NaN)
                return;

            float size = CameraControl.Instance.MyCamera.orthographicSize;

            if (IsLowRes)
            {
                if (size > LowResScale)
                    return;

                IsLowRes = false;
                HighRes();
            }
            else
            {
                if (size < LowResScale)
                    return;

                IsLowRes = true;
                LowRes();
            }
        }

        private void OnMouseDown()
        {
            if (SceneManager.sceneCount > 1)
                return;

            ObjectDataLoader.celestialBody = Target;

            SceneManager.LoadScene("ObjectData", LoadSceneMode.Additive);

            if (CameraFocus)
                CameraControl.Instance.Focus(this);
        }

        public virtual void OnUpdate() { }
        public abstract void Spawn(Vector2 pos, int? seed);

        public static float GetFairSize(float Size, float minSize, float maxSize)
        {
            return (float)GetFairSize((double)Size, minSize, maxSize);
        }

        public static double GetFairSize(double Size, double minSize, double maxSize)
        {
            double SizeDifference = maxSize - minSize;
            return ((Size - minSize) / SizeDifference) + 1;
        }

        public static float GetFairSizeCurve(double size, double coefficient, double pwrBase)
        {
            // 2 should be twice as much as one
            double one = RandomNum.CurveAt(1, coefficient, pwrBase);
            double two = RandomNum.CurveAt(2, coefficient, pwrBase);

            // one * 2 * m = two
            // one * 2 * m / 2 = 1
            double justify = two / one / 2;
            return (float)(justify * size);
        }

        protected virtual void Destroyed() { }

        protected virtual void LowRes() { }
        protected virtual void HighRes() { }

        private void OnDestroy()
        {
            IsDestroyed = true;
            Destroyed();
            cameraLerpTarget = null;
        }
    }
}
