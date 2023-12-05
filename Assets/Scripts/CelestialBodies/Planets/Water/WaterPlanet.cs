using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class WaterPlanet : Planet
    {
        public const double MinScale = 1000, MaxScale = 12000;
        public const double MinMass = 3e22, MaxMass = 4e24;

        public override string TypeString => IsOcean ? "Ocean" : "Water Planet";

        public override string PlanetTargetScene => "WaterPlanet";

        public override bool Circular => true;

        public override string ObjectFilePos => "Objects/Planet/WaterPlanet";
        public bool IsOcean;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        public void SetOceanName()
        {
            Name = "Sea of " + RandomNum.GetWord(2, RandomNumberGenerator); // geenyus
        }
    }
}
