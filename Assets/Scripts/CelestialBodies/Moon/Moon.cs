using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class Moon : Planet
    {
        public const double MinMass = 3e22, MaxMass = 4e24;
        public Planet planet;
        public override string TypeString => "Moon";

        public override string TravelTarget => PlanetTargetScene;

        public override bool Circular => true;

        public override string ObjectFilePos => "Objects/Moon";

        public override string PlanetTargetScene => "RockyPlanet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = RandomNum.Get(1000, 3000, RandomNumberGenerator);
            Name = RandomNum.GetPlanetName(RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        public override string GetBonusTypes()
        {
            return "Planet - " + planet.Name;
        }
    }
}
