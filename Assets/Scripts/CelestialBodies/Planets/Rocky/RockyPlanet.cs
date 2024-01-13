using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class RockyPlanet : Planet
    {
        public const float MinScale = 1000, MaxScale = 12000;
        public const double MinMass = 3e24, MaxMass = 4e26;

        public override string ObjectFilePos => "Planets/Rocky";
        public override bool Circular => true;
        public override string TypeString => "Rocky Planet";

        public override string PlanetTargetScene => "RockyPlanet";

        public ColorHSV RockColor;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            if (Seed == Star.Mercury)
            {
                Name = "Mercury";
                Radius = 2439 * Measurement.Km;
                Mass = 3.285e23;
                RockColor = new ColorHSV(13 / 360f, 0, .5f);
            }
            else if (Seed == Star.Mars)
            {
                Name = "Mars";
                Radius = 3389 * Measurement.Km;
                Mass = 6.39e23 * Measurement.Kg;
                RockColor = new ColorHSV(13 / 360f, 1, .5f);
            }
            else
            {
                Name = GenerateName();
                Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
                Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
                RockColor = new ColorHSV(13 / 360f, RandomNum.GetFloat(1, RandomNumberGenerator), .5f);
            }
        }
    }
}
