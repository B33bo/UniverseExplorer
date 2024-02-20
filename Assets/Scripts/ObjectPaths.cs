using System;
using UnityEngine;
using Universe.Animals;

namespace Universe
{
    [CreateAssetMenu(fileName = "Paths", menuName = "Universe/ObjectPaths")]
    public class ObjectPaths : ScriptableObject
    {
        private static ObjectPaths _instance;
        public static ObjectPaths Instance { get
            {
                if (_instance == null)
                    _instance = Resources.Load<ObjectPaths>("Paths");
                return _instance;
            }
        }

        public ObjectInfo<CelestialBodyRenderer>[] objects;
        public ObjectInfo<Animal>[] animals;
        private float animalSpawnChanceTotalFrequency;

        public int GetRandomAnimal(System.Random rand)
        {
            if (animalSpawnChanceTotalFrequency == 0)
            {
                for (int i = 0; i < animals.Length; i++)
                    animalSpawnChanceTotalFrequency += animals[i].Object.SpawnChance;
            }

            float targetFrequency = RandomNum.GetFloat(animalSpawnChanceTotalFrequency, rand);

            for (int i = 0; i < animals.Length; i++)
            {
                targetFrequency -= animals[i].Object.SpawnChance;
                if (targetFrequency < 0) return i;
            }
            return -1;
        }

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

        public Animal GetAnimal(string path)
        {
            path = path.ToLower();
            for (int i = 0; i < animals.Length; i++)
            {
                if (animals[i].Path.ToLower() == path)
                    return animals[i].Object;
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
