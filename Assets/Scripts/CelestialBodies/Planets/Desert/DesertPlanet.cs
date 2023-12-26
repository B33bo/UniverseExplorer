using UnityEngine;

namespace Universe.CelestialBodies.Planets.Desert
{
    public class DesertPlanet : Planet
    {
        public const double minRadius = 2000 * Measurement.Km, maxRadius = 5000 * Measurement.Km;
        public override string ObjectFilePos => "Objects/Planet/Desert";

        public override string PlanetTargetScene => "Desert";

        public override string TypeString => "Desert Planet";

        public override void Create(Vector2 pos)
        {
            Name = GenerateName();
            Position = pos;
            Radius = RandomNum.Get(minRadius, maxRadius, RandomNumberGenerator);
            Mass = Radius * Radius * 11; // hehe
            waterColor = RandomNum.GetColor(RandomNumberGenerator);
        }
    }
}
