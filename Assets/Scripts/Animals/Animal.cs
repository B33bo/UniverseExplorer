using UnityEngine;

namespace Universe.Animals
{
    public abstract class Animal : CelestialBody
    {
        public override string TravelTarget => string.Empty;
        public abstract string DataPath { get; }

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            throw new System.NotImplementedException();
        }

        public abstract void CreateSpecies();
        public abstract void CreateAnimal(Animal speciesPrefab);
    }
}
