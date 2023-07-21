using UnityEngine;

namespace Btools.DevConsole
{
    public class DebugCam : MonoBehaviour
    {
        private float speed = 1;
        private Camera cam;
        private Camera oldMain;

        private void Awake()
        {
            oldMain = Camera.main;
            oldMain.tag = "Untagged";
            tag = "MainCamera";
            cam = GetComponent<Camera>();
        }

        private void OnDestroy()
        {
            oldMain.tag = "MainCamera";
        }

        private void Update()
        {
            int directionX = 0;
            if (Input.GetKey(KeyCode.L))
                directionX = 1;
            if (Input.GetKey(KeyCode.J))
                directionX -= 1;

            int directionY = 0;
            if (Input.GetKey(KeyCode.I))
                directionY = 1;
            if (Input.GetKey(KeyCode.K))
                directionY -= 1;

            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
                speed++;
            if (Input.GetKeyDown(KeyCode.Minus))
                speed--;

            if (Input.GetKeyDown(KeyCode.O))
                cam.orthographicSize += speed * cam.orthographicSize * .2f;
            if (Input.GetKeyDown(KeyCode.U))
                cam.orthographicSize -= speed * cam.orthographicSize * .2f;

            if (Input.GetKeyDown(KeyCode.X))
                Destroy(gameObject);

            transform.position += speed * Time.deltaTime * new Vector3(directionX, directionY);
        }
    }
}
