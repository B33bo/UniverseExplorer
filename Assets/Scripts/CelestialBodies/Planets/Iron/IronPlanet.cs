using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class IronPlanet : Planet
    {
        public const double MinScale = 100 * Measurement.Km, MaxScale = 6371 * Measurement.Km;
        public const double Density = 7874;

        public override string ObjectFilePos => "Planet/Iron";

        public override string PlanetTargetScene => "IronPlanet";

        public override string TypeString => "Iron Planet";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = GenerateName();

            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Mass = Mathf.PI * Radius * Radius * Density;
        }
    }
}