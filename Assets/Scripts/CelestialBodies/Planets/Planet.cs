using System.Reflection;
using UnityEngine;
using Universe.Animals;

namespace Universe.CelestialBodies.Planets
{
    public abstract class Planet : CelestialBody
    {
        public override bool Circular => true;
        public abstract string ObjectFilePos { get; }
        public abstract string PlanetTargetScene { get; }
        public override string TravelTarget => PlanetTargetScene;
        public MoonRenderer[] moons;
        public Star sun;
        public float age = -1;
        public Color waterColor = new(.15f, .002f, 1);
        public (AnimalSpecies, string)[] Animals;

        public string GenerateName()
        {
            return RandomNum.GetWord(3, RandomNumberGenerator) + " " + RandomNum.GetString(1, RandomNumberGenerator);
        }

        public void SpawnMoons(Transform transform)
        {
            if (sun == null)
                return;
            moons = new MoonRenderer[RandomNumberGenerator.Next(0, 3)];

            for (int i = 0; i < moons.Length; i++)
            {
                MoonRenderer newMoon = Object.Instantiate((MoonRenderer)ObjectPaths.Instance.GetCelestialBody("Planets/Moon"), transform);
                newMoon.Spawn(Vector2.zero, RandomNumberGenerator.Next());
                moons[i] = newMoon;
                (moons[i].Target as Moon).planet = this;
                newMoon.Scale /= (float)sun.trueRadius;

                float distance = RandomNum.GetFloat(1, 2, newMoon.Target.RandomNumberGenerator);
                float rotation = RandomNum.GetFloat(0, 2 * Mathf.PI, newMoon.Target.RandomNumberGenerator);

                newMoon.Target.Position = new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation)) * distance;
            }
        }

        public void LoadAnimals()
        {
            LoadAnimals(5);
        }

        public virtual void LoadAnimals(int count)
        {
            Animals = new (AnimalSpecies, string)[count];
            for (int i = 0; i < Animals.Length; i++)
            {
                Animals[i] = GetRandomSpecies();
            }
        }

        public virtual (AnimalSpecies, string) GetRandomSpecies()
        {
            int index = ObjectPaths.Instance.GetRandomAnimal(RandomNumberGenerator);
            var animalInfo = ObjectPaths.Instance.animals[index];
            var animalType = animalInfo.Object.GetType().GetCustomAttribute<AnimalAttribute>().SpeciesType;

            AnimalSpecies species = System.Activator.CreateInstance(animalType) as AnimalSpecies;
            species.Create(new System.Random(RandomNumberGenerator.Next()));
            return (species, animalInfo.Path);
        }

        public override string GetBonusTypes()
        {
            if (sun == null)
                return "";
            return "Sun - " + sun.Name;
        }
    }
}
