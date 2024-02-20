using System;

namespace Universe.Animals
{
    public abstract class AnimalSpecies : Species
    {
        public Leg[] Legs = Array.Empty<Leg>();
        public Eye[] Eyes = Array.Empty<Eye>();
        public Torso torso;
    }
}
