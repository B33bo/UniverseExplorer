using Btools.DevConsole;
using System.Collections.Generic;
using UnityEngine;
using Universe.CelestialBodies;

namespace Universe
{
    public static class BodyManager
    {
        private static Stack<string> ScenesVisited = new Stack<string>();
        private static Stack<CelestialBody> BodiesVisited = new Stack<CelestialBody>();
        private static Stack<Vector2> CameraPosition = new Stack<Vector2>();

        public static CelestialBody Parent;
        public static CelestialBodies.Universe Universe { get; private set; }
        public static int SceneVisitedCount => ScenesVisited.Count;

        public delegate void Loaded(string Scene);
        public static event Loaded OnSceneLoad;

        public static void RegisterSceneLoad(string Scene)
        {
            Debug.Log($"Loaded Scene {Scene}");
            OnSceneLoad?.Invoke(Scene);

            if (Parent is CelestialBodies.Universe u)
                Universe = u;
        }

        public static CelestialBody[] GetPath => BodiesVisited.ToArray();

        public static int GetSeed() =>
            Parent is null ? 0 : Parent.Seed;

        public static void ReloadCommands()
        {
            var paths = Resources.Load<ObjectPaths>("Paths");
            System.Text.StringBuilder objectListArgs = new();

            for (int i = 0; i < paths.objects.Length; i++)
                objectListArgs.Append(paths.objects[i].Path + "|");

            DevCommands.RegisterVar(new DevConsoleVariable("parent", "list the info of the parent", typeof(CelestialBody),
                () => Parent.ToString()));

            DevCommands.Register("spawn", "spawn an object", Spawn, new string[] { objectListArgs.ToString() });
            DevCommands.RegisterVar(new DevConsoleVariable("Time", "Time of simulation", typeof(float),
                () => GlobalTime.Time, x => GlobalTime.SetTime(float.Parse(x))));

            static string Spawn(string[] parameters)
            {
                var prefab = Resources.Load<ObjectPaths>("Paths").GetCelestialBody(parameters[1]);
                var obj = Object.Instantiate(prefab);

                if (parameters.Length < 3)
                    obj.Spawn(Camera.main.transform.position, null);
                else
                    obj.Spawn(Camera.main.transform.position, int.Parse(parameters[2]));

                if (parameters.Length >= 4 && parameters[3] == "true")
                    obj.gameObject.AddComponent<BoxCollider2D>();

                return obj.Target.ToString();
            }
        }

        public static void LoadSceneBackwards()
        {
            if (Parent == Universe)
                Universe = null;
            Parent = BodiesVisited.Pop();
            UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesVisited.Pop());
            CameraControl.Instance.Position = CameraPosition.Pop();
        }

        public static void TravelTo(string position)
        {
            Debug.Log($"Loading {position}");

            CameraPosition.Push(CameraControl.Instance.Position);
            CameraControl.Instance.UnFocus();
            CameraControl.Instance.Position = Vector3.zero;

            BodiesVisited.Push(Parent);

            ScenesVisited.Push(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(position);
        }
    }
}
