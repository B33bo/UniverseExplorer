using System;

namespace Universe.Animals
{
    public abstract class Species
    {
        public abstract string AnimalName { get; }
        public string speciesName;
        public float neutralEnergyCost = 1;
        public float minDiet = 0, maxDiet = 1;
        public float maxEnergy = 150;
        public float maxHealth = 100;
        public float walkSpeed = 1;

        public float Sprint = 2.5f;
        public float Run = 2;

        public abstract void Create(Random rand);
    }
}
