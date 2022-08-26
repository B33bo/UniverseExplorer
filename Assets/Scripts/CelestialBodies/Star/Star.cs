using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class Star : CelestialBody
    {
        public const double MinMass = 1.5912E+29, MaxMass = 2.9835E+32;
        public const double MinSize = 1e6, MaxSize = 10_000_000;
        public static string[] StarNames = null;

        public Color starColor;

        public override void Create(Vector2 position)
        {
            if (StarNames is null)
                StarNames = Resources.Load<TextAsset>("StarNames").text.Split('\n');

            Position = position;
            Name = StarNames[RandomNum.Get(0, StarNames.Length, RandomNumberGenerator)].Trim() + " " + RandomNum.GetString(1, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
            Radius = RandomNum.Get(MinSize, MaxSize, RandomNumberGenerator);
            Temperature = RandomNum.Get(3000, 10000, RandomNumberGenerator);
        }

        public override string GetBonusTypes() =>
            "Temperature - " + Temperature;

        public override bool Circular => true;
        public override string TravelTarget => BodyManager.Parent is Star ? "Star" : "SolarSystem";
        public override string TypeString => "Star";
        public double Temperature;
    }
}
