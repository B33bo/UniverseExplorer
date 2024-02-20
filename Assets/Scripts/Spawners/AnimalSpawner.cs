using System;
using UnityEngine;

namespace Universe.AnimalsOld
{
    public class AnimalSpawner : MonoBehaviour
    {
        public static AnimalSpawner InsideAnimals { get; private set; }
        public static AnimalSpawner WalkingAnimals { get; private set; }

        [SerializeField]
        private AnimalRenderer[] animalPrefabs;

        [SerializeField]
        private float[] weights;

        [SerializeField]
        private float chanceOfSpawning = 10;

        [SerializeField]
        private Vector2Int AnimalCountBounds;

        [SerializeField]
        private bool IsInside = true;

        public static AnimalOld[] Animals;
        private static string[] AnimalRendererTypes;

        private void Awake()
        {
            if (IsInside)
                InsideAnimals = this;
            else
                WalkingAnimals = this;

            float weightTotal = 0;
            for (int i = 0; i < weights.Length; i++)
                weightTotal += weights[i];

            System.Random rand = new System.Random(BodyManager.GetSeed());
            Animals = new AnimalOld[RandomNum.Get(AnimalCountBounds.x, AnimalCountBounds.y, rand)];
            AnimalRendererTypes = new string[Animals.Length];

            for (int i = 0; i < Animals.Length; i++)
            {
                int index = RandomNum.GetIndexFromWeights(weights, rand);

                AnimalOld newAnimal = (AnimalOld)Activator.CreateInstance(animalPrefabs[index].AnimalType);
                newAnimal.SetSeed(rand.Next());
                newAnimal.CreateSpecies();

                Animals[i] = newAnimal;
                AnimalRendererTypes[i] = newAnimal.DataPath;
            }
        }

        public AnimalRenderer SpawnAnimalAt(Vector2 position, int seed)
        {
            if (Animals.Length == 0)
                return null;

            if (AnimalRenderer.seedsSpawned.Contains(seed))
                return null;

            System.Random rand = new System.Random(seed);

            if (RandomNum.Get(0, chanceOfSpawning, rand) > 1)
                return null;

            int index = RandomNum.Get(0, Animals.Length, rand);

            if (Animals[index] == null)
                return null;

            var animalRenderer = Instantiate(Resources.Load<AnimalRenderer>(AnimalRendererTypes[index]));
            animalRenderer.Spawn(position, seed, Animals[index]);
            AnimalRenderer.seedsSpawned.Add(seed);
            return animalRenderer;
        }
    }
}
