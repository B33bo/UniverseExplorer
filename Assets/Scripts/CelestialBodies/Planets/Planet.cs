using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public abstract class Planet : CelestialBody
    {
        public override bool Circular => true;
        public abstract string ObjectFilePos { get; }
        public abstract string PlanetTargetScene { get; }
        public override string TravelTarget => BodyManager.Parent is Planet ? PlanetTargetScene : "PlanetOrbiter";
        public Moon[] moons;
        public Star sun;
        public float age = -1;

        public string GenerateName()
        {
            return RandomNum.GetPlanetName(RandomNumberGenerator) + " " + RandomNum.GetString(1, RandomNumberGenerator);
        }
    }
}
