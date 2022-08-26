using UnityEngine;
using UnityEngine.SceneManagement;

namespace Btools.utils
{
    public static class GameObjectUtils
    {
        private static Scene? _dontdestroyonload;
        public static Scene DontDestroyOnLoadScene
        {
            get
            {
                if (_dontdestroyonload.HasValue)
                    return _dontdestroyonload.Value;

                GameObject g = new GameObject("DontDestroyOnLoadTemp");
                MonoBehaviour.DontDestroyOnLoad(g);
                _dontdestroyonload = g.scene;
                return _dontdestroyonload.Value;
            }
        }

        public static GameObject FindPath(string path)
        {
            return FindPath(path.Split('/'));
        }

        public static GameObject FindPath(params string[] path)
        {
            if (TryFindPath(out GameObject value, path))
                return value;

            if (path.Length < 2)
                throw new System.FormatException("path was not of correct format");
            if (!SceneManager.GetSceneByName(path[0]).IsValid())
                throw new System.InvalidOperationException("scene name was invalid");
            throw new System.ArgumentException("could not find position of path");
        }

        public static bool TryFindPath(out GameObject gameObject, params string[] path)
        {
            gameObject = null;
            if (path.Length == 1 && path[0] == "null")
                return true;

            if (path.Length < 2)
                return false;

            Scene scene;
            if (path[0] == "DontDestroyOnLoad")
                scene = DontDestroyOnLoadScene;
            else
                scene = SceneManager.GetSceneByName(path[0]);

            if (!scene.IsValid())
                return false;

            var rootObjects = scene.GetRootGameObjects();

            Transform current = null;
            for (int i = 0; i < rootObjects.Length; i++)
            {
                if (rootObjects[i].name == path[1])
                {
                    current = rootObjects[i].transform;
                    break;
                }

                if (i == rootObjects.Length - 1)
                    return false;
            }

            for (int i = 2; i < path.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < current.childCount; j++)
                {
                    var ChildFound = current.GetChild(j);
                    if (ChildFound.name != path[i])
                        continue;

                    current = ChildFound;
                    found = true;
                    break;
                }

                if (!found)
                {
                    if (i == path.Length - 1 && path[i] == "")
                        break;
                    return false;
                }
            }

            gameObject = current.gameObject;
            return true;
        }

        public static string GetGameObjectPath(this GameObject gameObject)
        {
            return gameObject.transform.GetGameObjectPath();
        }

        public static string GetGameObjectPath(this Transform transform)
        {
            string path = "";
            Transform current = transform;

            while (current)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            path = transform.gameObject.scene.name + "/" + path;
            return path;
        }
    }
}
