using System.Collections.Generic;
using UnityEngine;
using Universe.Animals;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class AnimalSpawner : MonoBehaviour
    {
        public AnimalSpawnerType animalSpawnerType;

        private static Dictionary<AnimalSpawnerType, AnimalSpawner> spawners = new();

        private void Awake()
        {
            if (spawners.ContainsKey(animalSpawnerType))
                spawners[animalSpawnerType] = this;
            else
                spawners.Add(animalSpawnerType, this);
        }

        public void Spawn(float x, float y)
        {
            (AnimalSpecies, string) animalInfo;
            if (BodyManager.Parent is Planet p)
                animalInfo = p.Animals[Random.Range(0, p.Animals.Length)];
            else
            {
                animalInfo = (new TillySpecies(), "tilly");
                animalInfo.Item1.Create(new System.Random());
            }

            var objectPrefab = ObjectPaths.Instance.GetAnimal(animalInfo.Item2);
            var newObject = Instantiate(objectPrefab, new Vector3(x, y + 3), Quaternion.identity);
            newObject.Init(animalInfo.Item1, new System.Random());
        }

        public static AnimalSpawner GetSpawner(AnimalSpawnerType animalSpawnerType) =>
            spawners[animalSpawnerType];
    }

    public enum AnimalSpawnerType
    {
        Ground,
        Water,
    }
}
