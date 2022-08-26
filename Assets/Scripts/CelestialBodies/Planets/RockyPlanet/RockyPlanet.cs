using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class RockyPlanet : Planet
    {
        public const float MinScale = 1000, MaxScale = 12000;
        public const double MinMass = 3e24, MaxMass = 4e26;

        public override string ObjectFilePos => "Objects/RockyPlanet";
        public override bool Circular => true;
        public override string TypeString => "Rocky Planet";

        public override string PlanetTargetScene => "RockyPlanet";

        public (float H, float S, float V) RockColor;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();

            RockColor = (13 / 360f, RandomNum.GetFloat(1, RandomNumberGenerator), .5f);
        }
    }
}
