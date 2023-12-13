using UnityEngine;
using Universe.CelestialBodies.Biomes;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class TerrestrialPlanet : Planet
    {
        public const int NorthAmerica = 0, SouthAmerica = 1, Africa = 2, Europe = 3, Asia = 4, Oceania = 5, Antarctica = 6;
        public const double MinScale = 4000 * Measurement.Km, MaxScale = 12000 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;

        public override string TypeString => "Terrestrial Planet";

        public override string PlanetTargetScene => "TerrestrialPlanet";

        public override bool Circular => true;

        public override string ObjectFilePos => "Objects/Planet/TerrestrialPlanet";
        public Continent[] continents;

        [InspectableVar("Water Level", Params = new object[] { 0, 1 })]
        public float waterLevel;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            if (Seed == Star.Earth)
            {
                Radius = 6371 * Measurement.Km;
                Name = "Earth";
                Mass = 5.972e24 * Measurement.Kg;
                GenerateEarth();
                return;
            }

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

        private void GenerateEarth()
        {
            continents = new Continent[7];

            for (int i = 0; i < continents.Length; i++)
            {
                Continent continent = new Continent();
                continent.SetSeed(i);
                continent.Create((Vector2)Position + RandomNum.GetVector(-.5f, .5f, RandomNumberGenerator));
                continent.planet = this;
                continents[i] = continent;

                Mass += continents[i].Mass;
            }
        }
    }
}
