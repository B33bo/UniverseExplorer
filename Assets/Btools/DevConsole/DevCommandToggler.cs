using UnityEngine;

namespace Btools.Components
{
    public class DevCommandToggler : MonoBehaviour
    {
        [SerializeField]
        private GameObject DevConsole;

        private static bool IsLoaded;

        private void Start()
        {
            if (IsLoaded)
            {
                Destroy(gameObject);
                return;
            }

            IsLoaded = true;
            DontDestroyOnLoad(this);

            DevConsole.SetActive(false);
            DevConsole.transform.position = Vector3.zero;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
                DevConsole.SetActive(!DevConsole.activeSelf);
        }
    }
}
