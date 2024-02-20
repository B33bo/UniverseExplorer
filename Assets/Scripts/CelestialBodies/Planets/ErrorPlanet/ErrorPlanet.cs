using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class ErrorPlanet : Planet
    {
        public override string ObjectFilePos => "Planets/Error";

        public override string PlanetTargetScene => "ErrorPlanet";

        public override string TypeString => "Error Planet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = GenerateName();
            LoadAnimals(0);
        }
    }
}
