using UnityEngine;
using Universe.CelestialBodies.Biomes;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanet : Planet
    {
        public const double MinScale = 4000 * Measurement.Km, MaxScale = 12000 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;

        public override string TypeString => "Terrestrial Planet";

        public override string PlanetTargetScene => "WaterPlanet";
        public override string TravelTarget => BodyManager.Parent is Planet ? PlanetTargetScene : "PlanetOrbiter";

        public override bool Circular => true;

        public override string ObjectFilePos => "Objects/Planet/TerrestrialPlanet";
        public Continent[] continents;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);

            continents = new Continent[RandomNum.Get(1, 10, RandomNumberGenerator)];

            for (int i = 0; i < continents.Length; i++)
            {
                Continent continent = new Continent();
                continent.SetSeed(RandomNum.Get(int.MinValue, int.MaxValue, RandomNumberGenerator));
                continent.Create((Vector2)Position + RandomNum.GetVector(-.5f, .5f, RandomNumberGenerator));
                continent.planet = this;
                continents[i] = continent;

                Mass += continents[i].Mass;
            }
        }
    }
}
