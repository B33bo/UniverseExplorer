using UnityEngine;
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

        public override string ObjectFilePos => "Planets/Terrestrial";

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
                waterColor = new(0, .15f, .71f);
                return;
            }

            if (RandomNum.GetBool(RandomNumberGenerator))
            {
                float r = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                float g = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                float b = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                waterColor += new Color(r, g, b);
            }
            else
            {
                waterColor = RandomNum.GetColor(RandomNumberGenerator);
            }

            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
            waterLevel = RandomNum.GetFloat(.5f, RandomNumberGenerator);
            LoadAnimals();
        }
    }
}
