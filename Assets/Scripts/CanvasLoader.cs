using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Universe
{
    public class CanvasLoader : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject blackHoleText;

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
                blackHoleText.SetActive(true);
                return;
            }

            button.interactable = true;
            blackHoleText.SetActive(false);

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
            float size = CameraControl.Instance.MyCamera.orthographicSize;
            size += amount;
            size = Mathf.Max(0.1f, size);
            CameraControl.Instance.MyCamera.orthographicSize = size;
        }
    }
}