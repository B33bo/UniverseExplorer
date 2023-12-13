using Btools.DevConsole;
using Btools.TimedEvents;
using TMPro;
using UnityEngine;

namespace Universe
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        private const bool AllowCameraRotate = false; // makes me dizzy soz
        public static CameraControl Instance { get; private set; }

        private Camera _camera;

        public float Height => 2f * MyCamera.orthographicSize;
        public float Width => Height * MyCamera.aspect;

        private Transform FocusTarget;

        [SerializeField]
        private CelestialBodies.CameraMoverRenderer cameraMoverRendererPrefab;
        private Rect _cameraBounds;
        private Vector2 cachedMovement;

        public Rect CameraBounds => _cameraBounds;

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

        [SerializeField]
        private float CamScale = 5;
        public event CameraPositionUpdate OnPositionUpdate;
        public event CameraFinishedLoading OnFinishedLoading;

        public delegate void CameraPositionUpdate(Rect bounds);
        public delegate void CameraFinishedLoading();

        private byte isLoading = 0;

        [SerializeField]
        private bool wasdMovement = true;

        private void Awake()
        {
            // So that instance doesn't get overriten, use the ??=
            Instance ??= this;
        }

        private void Start()
        {
            PositionText = GameObject.Find("PositionText").GetComponent<TextMeshProUGUI>();
            DevCommands.Register("position", "set the position of the camera", PositionCommand, "x", "y");
            DevCommands.RegisterVar(new DevConsoleVariable("speed", "the speed of the camera", typeof(float),
                () => Speed,
                x => Speed = float.Parse(x)));

            BodyManager.OnSceneLoad += _ =>
            {
                isLoading++;
                MyCamera.orthographicSize = CamScale;
                MyCamera.backgroundColor = Color.black;
                Timed.RunAfterFrames(() => { isLoading--; OnFinishedLoading?.Invoke(); }, 1);
            };

            grid.EnableDevCommand();
        }

        private string PositionCommand(string[] parameters)
        {
            if (parameters.Length > 1)
                Position = new Vector2(float.Parse(parameters[1]), float.Parse(parameters[2]));
            return Position.ToString();
        }

        private void KeepFocusing()
        {
            Vector3 position = FocusTarget.position;
            position.z = -10;
            transform.SetPositionAndRotation(position, AllowCameraRotate ? FocusTarget.rotation : transform.rotation);
        }

        private void Move()
        {
            if (FocusTarget)
            {
                KeepFocusing();
                return;
            }

            Vector2 movement = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Application.isMobilePlatform)
                movement = MobileMovement();

            MoveByVector(Speed * Time.deltaTime * movement);

            if (PositionText)
                PositionText.text = Position.ToString();

            if (Input.GetMouseButtonDown(1))
            {
                Vector2 position = MyCamera.ScreenToWorldPoint(Input.mousePosition);
                var cameraMoveRenderer = Instantiate(cameraMoverRendererPrefab, position, Quaternion.identity);
                cameraMoveRenderer.Spawn(position, null);
                Focus(cameraMoveRenderer);
            }
        }

        private void MoveByVector(Vector2 movement)
        {
            /* it's done like this because for very large values of the position,
             * the change in movement for each frame is below the floating point precision, resulting in 0 net movement.
             * This is the fix. It results in jagged movement but that's better than no movement. Also it's unavoidable
             */

            cachedMovement += movement;
            Vector2 oldPosition = Position;
            Position += cachedMovement;

            if (Position.x != oldPosition.x)
                cachedMovement.x = 0;
            if (Position.y != oldPosition.y)
                cachedMovement.y = 0;
        }

        private void Update()
        {
            Rect bounds()
            {
                Vector2 middle = transform.position;
                Vector2 scale = new Vector2(Width, Height);
                Vector2 bottomLeft = (middle - scale / 2);
                return new Rect(bottomLeft, scale);
            }

            GlobalTime.MaybeInvokeImportantUpdate();

            if (!wasdMovement)
                return;

            _cameraBounds = bounds();
            Move();

            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y == 0)
                    mouseScrollDelta = 0;
                else if (Input.mouseScrollDelta.y > 0)
                    mouseScrollDelta = 1.1f;
                else
                    mouseScrollDelta = .9f;
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
            Zoom(mouseScrollDelta);

            if (mouseScrollDelta != 0 && lastPosition == (Vector2)transform.position)
                return;

            if ((Vector2)transform.position != lastPosition || MyCamera.orthographicSize != lastSize)
                OnPositionUpdate?.Invoke(CameraBounds);

            lastSize = MyCamera.orthographicSize;
            lastPosition = transform.position;
        }

        public void Zoom(float amount)
        {
            if (amount == 0)
                return;
            CamScale /= amount;
            MyCamera.orthographicSize = CamScale;
            mouseScrollDelta = 0;
        }

        public void SetScale(float amount)
        {
            CamScale = amount;
            MyCamera.orthographicSize = CamScale;
            mouseScrollDelta = 0;
        }

        private float BeforeLerpCamScale = 0;

        public void Focus(CelestialBodyRenderer focus)
        {
            if (!wasdMovement)
                return;

            float targetScale = focus.cameraLerpSize;

            if (focus.cameraLerpMultiplyBySize)
                targetScale *= focus.transform.localScale.x;

            float startCamLerp = MyCamera.orthographicSize;

            Vector2 startPosition = transform.position;

            Quaternion startRotation = Quaternion.identity;

            BeforeLerpCamScale = MyCamera.orthographicSize;

            float t = 0;
            Timed.RepeatUntil(() =>
            {
                if (isLoading > 0)
                    return true;

                if (focus is null)
                    return true;
                if (focus.IsDestroyed)
                    return true;
                if (focus.cameraLerpTarget is null)
                    return true;

                t += Time.deltaTime * 2;

                Vector3 position = Vector3.Lerp(startPosition, focus.cameraLerpTarget.position, t);
                position.z = -10;

                Quaternion rotation = AllowCameraRotate ? Quaternion.Lerp(startRotation, focus.cameraLerpTarget.rotation, t) : startRotation;

                if (targetScale >= 0)
                    SetScale(Mathf.Lerp(startCamLerp, targetScale, t));

                transform.SetPositionAndRotation(position, rotation);

                if (t >= 1)
                    FocusTarget = focus.cameraLerpTarget;
                return t >= 1;
            });
        }

        public void UnFocus()
        {
            if (!wasdMovement)
                return;
            if (FocusTarget is null)
                return;
            FocusTarget = null;

            float targetScale = BeforeLerpCamScale;
            float startCamLerp = MyCamera.orthographicSize;

            Quaternion startRotation = transform.rotation;

            float t = 0;
            Timed.RepeatUntil(() =>
            {
                t += Time.deltaTime * 2;
                SetScale(Mathf.Lerp(startCamLerp, targetScale, t));
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
                return t >= 1;
            });
        }
    }
}
