using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Universe
{
    public class CanvasLoader : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI fps;

        public void LoadBackwards()
        {
            BodyManager.LoadSceneBackwards();
        }

        private void Start()
        {
            BodyManager.OnSceneLoad += SceneLoaded;
            StartCoroutine(FPSUpdater());
            SceneLoaded(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void SceneLoaded(string Scene)
        {
            if (Scene == "BlackHole")
            {
                button.interactable = false;
                return;
            }

            button.interactable = true;

            if (BodyManager.SceneVisitedCount < 1)
                button.gameObject.SetActive(false);
            else
                button.gameObject.SetActive(true);
        }

        private IEnumerator FPSUpdater()
        {
            const float timeBetweenUpdates = .5f;
            int beforeFrames = Time.frameCount;
            yield return new WaitForSeconds(timeBetweenUpdates);
            fps.text = System.Math.Round((Time.frameCount - beforeFrames) / timeBetweenUpdates).ToString();
            StartCoroutine(FPSUpdater());
        }

        public void CameraZoom(float amount)
        {
            CameraControl.Instance.Zoom(amount);
            /*float size = CameraControl.Instance.MyCamera.orthographicSize;
            size += amount;
            size = Mathf.Max(0.1f, size);
            CameraControl.Instance.MyCamera.orthographicSize = size;*/
        }
    }
}