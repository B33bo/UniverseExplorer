using System;
using UnityEngine;

namespace Universe
{
    [CreateAssetMenu(fileName = "Paths", menuName = "Universe/ObjectPaths")]
    public class ObjectPaths : ScriptableObject
    {
        public ObjectInfo<CelestialBodyRenderer>[] objects;

        public CelestialBodyRenderer GetCelestialBody(string path)
        {
            path = path.ToLower();
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].Path.ToLower() == path)
                    return objects[i].Object;
            }
            return null;
        }

        [Serializable]
        public class ObjectInfo<T>
        {
            public string Path;
            public T Object;

            public ObjectInfo(T obj, string path)
            {
                Object = obj;
                Path = path;
            }

            public static implicit operator ObjectInfo<T>(T data)
            {
                return new ObjectInfo<T>(data, data.ToString());
            }
        }

        public enum VariableType
        {
            String,
            Int,
            Float,
            Color,
        }
    }
}
