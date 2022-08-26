using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe
{
    public abstract class CelestialBodyRenderer : MonoBehaviour
    {
        public CelestialBody Target { get; set; }

        public bool CameraFocus = true;

        public Transform cameraLerpTarget;
        public float cameraLerpSize;
        public bool cameraLerpMultiplyBySize;

        private void Update()
        {
#if UNITY_EDITOR
            if (Target is null)
                Debug.LogError("Target is null on object " + name);
#endif
            transform.localPosition = Target.Position;
            name = Target.Name;
            OnUpdate();
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

        public virtual void OnUpdate(){}
        public abstract void Spawn(Vector2 pos, int? seed);

        public static float GetFairSize(float Size, float minSize, float maxSize)
        {
            float SizeDifference = maxSize - minSize;
            return (float)((Size - minSize) / SizeDifference) + 1;
        }

        public virtual void Destroyed() { }

        private void OnDestroy()
        {
            Destroyed();
            cameraLerpTarget = null;
        }
    }
}
