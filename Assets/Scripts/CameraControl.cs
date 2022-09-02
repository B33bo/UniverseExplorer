using UnityEngine;
using TMPro;
using Btools.DevConsole;
using Btools.TimedEvents;
using UnityEngine.SceneManagement;

namespace Universe
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        public static CameraControl Instance { get; private set; }

        private Camera _camera;

        public float Height => 2f * MyCamera.orthographicSize;
        public float Width => Height * MyCamera.aspect;

        private Transform FocusTarget;

        [SerializeField]
        private CelestialBodies.CameraMoverRenderer cameraMoverRendererPrefab;

        public Rect CameraBounds
        {
            get
            {
                Vector2 middle = transform.position;
                Vector2 scale = new Vector2(Width, Height);
                Vector2 bottomLeft = (middle - scale / 2);
                return new Rect(bottomLeft, scale);
            }
        }

        public Camera MyCamera
        {
            get
            {
                if (_camera is null)
                    _camera = GetComponent<Camera>();
                return _camera;
            }
        }

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y, -10);
        }

        public float Speed = 5;

        [SerializeField]
        private GridDisplay grid;

        private TextMeshProUGUI PositionText;
        private float CamScale = 5;
        public event CameraPositionUpdate OnPositionUpdate;

        public delegate void CameraPositionUpdate(Rect bounds);

        private byte isLoading = 0;

        private void Start()
        {
            Instance = this;
            PositionText = GameObject.Find("PositionText").GetComponent<TextMeshProUGUI>();
            DevCommands.Register("position", "set the position of the camera", PositionCommand, "x", "y");
            DevCommands.RegisterVar(new DevConsoleVariable("speed", "the speed of the camera", typeof(float),
                () => Speed,
                x => Speed = float.Parse(x)));

            BodyManager.OnSceneLoad += _ =>
            {
                MyCamera.orthographicSize = CamScale;
                transform.position = new Vector3(0, 0, -10);
                MyCamera.backgroundColor = Color.black;
                isLoading++;
                Timed.RunAfterFrames(() => isLoading--, 1);
            };

            grid.EnableDevCommand();
        }

        private string PositionCommand(string[] parameters)
        {
            if (parameters.Length > 1)
                Position = new Vector2(float.Parse(parameters[1]), float.Parse(parameters[2]));
            return Position.ToString();
        }

        private void Update()
        {
            if (FocusTarget is null)
            {
                Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (Application.isMobilePlatform)
                    movement = MobileMovement();

                Position += Speed * Time.deltaTime * movement;
                PositionText.text = Position.ToString();
            }
            else
            {
                if (!FocusTarget)
                {
                    FocusTarget = null;
                    return;
                }
                Vector3 position = FocusTarget.position;
                position.z = -10;
                transform.position = position;
            }

            if (Input.mouseScrollDelta.y != 0)
                mouseScrollDelta = 
                    Input.mouseScrollDelta.y == 0 ? 0 : 
                    Input.mouseScrollDelta.y > 0 ? 1.1f :
                    .9f;

            if (Input.GetMouseButtonDown(1))
            {
                Vector2 position = MyCamera.ScreenToWorldPoint(Input.mousePosition);
                var cameraMoveRenderer = Instantiate(cameraMoverRendererPrefab, position, Quaternion.identity);
                cameraMoveRenderer.Spawn(position, null);
                Focus(cameraMoveRenderer);
            }
        }

        private Vector2 MobileMovement()
        {
            if (Input.touchCount == 0)
                return Vector2.zero;

            Vector2 mousePos = Input.touches[0].position / new Vector2(Screen.width, Screen.height);
            mousePos.x = (mousePos.x - .5f) * 2;
            mousePos.y = (mousePos.y - .5f) * 2;

            if (mousePos.x < .2f && mousePos.x > -.2f)
                mousePos.x = 0;
            if (mousePos.y < .2f && mousePos.y > -.2f)
                mousePos.y = 0;

            return mousePos * 3;
        }

        float mouseScrollDelta = 0;
        Vector2 lastPosition;
        float lastSize;

        private void FixedUpdate()
        {
            TryUpdateScroll();

            if (mouseScrollDelta != 0 && lastPosition == (Vector2)transform.position)
                return;

            if ((Vector2)transform.position != lastPosition || MyCamera.orthographicSize != lastSize)
                OnPositionUpdate?.Invoke(CameraBounds);

            lastSize = MyCamera.orthographicSize;
            lastPosition = transform.position;
        }

        private void TryUpdateScroll()
        {
            if (mouseScrollDelta == 0)
                return;
            CamScale /= mouseScrollDelta * 1;//+= -mouseScrollDelta * Time.fixedDeltaTime * 100;
            //CamScale = Mathf.Clamp(CamScale, 0.001f, float.PositiveInfinity);
            MyCamera.orthographicSize = CamScale;
            mouseScrollDelta = 0;
        }

        private float BeforeLerpCamScale = 0;

        public void Focus(CelestialBodyRenderer focus)
        {
            float targetScale = focus.cameraLerpSize;

            if (focus.cameraLerpMultiplyBySize)
                targetScale *= focus.transform.localScale.x;

            float startCamLerp = MyCamera.orthographicSize;

            Vector2 startPosition = transform.position;
            Vector3 newPosition = focus.transform.position;
            newPosition.z = -10;

            BeforeLerpCamScale = MyCamera.orthographicSize;

            float t = 0;
            Timed.RepeatUntil(() =>
            {
                if (isLoading > 0)
                    return true;

                if (focus is null)
                    return true;
                if (focus.cameraLerpTarget is null)
                    return true;

                t += Time.deltaTime * 2;

                Vector3 position = Vector3.Lerp(startPosition, focus.cameraLerpTarget.position, t);
                position.z = -10;

                if (targetScale >= 0)
                    _camera.orthographicSize = Mathf.Lerp(startCamLerp, targetScale, t);
                transform.position = position;

                if (t >= 1)
                    FocusTarget = focus.cameraLerpTarget;
                return t >= 1;
            });
        }

        public void UnFocus()
        {
            if (FocusTarget is null)
                return;
            FocusTarget = null;

            float targetScale = BeforeLerpCamScale;
            float startCamLerp = MyCamera.orthographicSize;

            float t = 0;
            Timed.RepeatUntil(() =>
            {
                t += Time.deltaTime * 2;
                MyCamera.orthographicSize = Mathf.Lerp(startCamLerp, targetScale, t);
                return t >= 1;
            });
        }
    }
}
