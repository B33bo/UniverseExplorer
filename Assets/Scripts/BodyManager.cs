using UnityEngine;
using System.Collections.Generic;
using Universe.CelestialBodies;
using Btools.DevConsole;

namespace Universe
{
    public static class BodyManager
    {
        private static Stack<string> ScenesVisited = new Stack<string>();
        private static Stack<CelestialBody> BodiesVisited = new Stack<CelestialBody>();
        public static CelestialBody Parent;
        public static int SceneVisitedCount => ScenesVisited.Count;

        public delegate void Loaded(string Scene);
        public static event Loaded OnSceneLoad;

        public static void InvokeSceneLoad(string Scene)
        {
            Debug.Log($"Loaded Scene {Scene}");
            OnSceneLoad?.Invoke(Scene);
        }

        public static CelestialBody[] GetPath => BodiesVisited.ToArray();

        public static int GetSeed() =>
            Parent is null ? 1 : Parent.Seed;

        public static void ReloadCommands()
        {
            DevCommands.Register("load", "load a scene", LoadCmd);
            DevCommands.RegisterVar(new DevConsoleVariable("parent", "list the info of the parent", typeof(CelestialBody),
                () => Parent.ToString()));

            DevCommands.Register("spawn", "spawn an object", Spawn);
            DevCommands.Register("objectlst", "list all objects", ObjectLst);

            string LoadCmd(string[] parameters)
            {
                TestBody testBody = new TestBody(Vector2.zero, int.Parse(parameters[1]), parameters[2]);
                ScenesVisited.Clear();
                BodiesVisited.Clear();
                Parent = testBody;
                UnityEngine.SceneManagement.SceneManager.LoadScene(parameters[2]);
                return testBody.ToString();
            }

            string Spawn(string[] parameters)
            {
                var prefab = Resources.Load<CelestialBodyRenderer>("Objects/" + parameters[1]);
                var obj = MonoBehaviour.Instantiate(prefab);

                if (parameters.Length == 2)
                    obj.Spawn(Camera.main.transform.position, null);
                else
                    obj.Spawn(Camera.main.transform.position, int.Parse(parameters[2]));

                return obj.Target.ToString();
            }

            string ObjectLst(string[] parameters)
            {
                string s = "";
                var objects = Resources.LoadAll<CelestialBodyRenderer>("Objects");
                for (int i = 0; i < objects.Length; i++)
                    s += objects[i].name + "\n";
                return s;
            }
        }

        public static void LoadSceneBackwards()
        {
            Parent = BodiesVisited.Pop();
            UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesVisited.Pop());
        }

        public static void TravelTo(string position)
        {
            Debug.Log($"Loading {position}");
            BodiesVisited.Push(Parent);
            ScenesVisited.Push(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(position);
        }
    }
}
