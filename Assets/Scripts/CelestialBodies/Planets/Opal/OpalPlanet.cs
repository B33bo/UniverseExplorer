using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class OpalPlanet : Planet
    {
        public const double MinScale = 1000 * Measurement.Km, MaxScale = 10000 * Measurement.Km;
        public const double Density = 2.09 * (Measurement.g / (Measurement.cm * Measurement.cm));

        public override string ObjectFilePos => "Planets/Opal";

        public override string PlanetTargetScene => "OpalPlanet";

        public override string TypeString => "Opal Planet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = GenerateName();
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = Radius * Radius * 3.14 * Density;
            waterColor = Color.HSVToRGB(RandomNum.GetFloat(1, RandomNumberGenerator), .2f, 1);
        }
    }
}
