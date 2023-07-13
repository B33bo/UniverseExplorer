using Btools.utils;
using System.Collections;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class Star : CelestialBody
    {
        public override bool Circular => true;
        public override string TravelTarget => "Star";
        public override string TypeString => "Star";
        public double Temperature;

        private static readonly string[] PlanetStrings = new string[] { "WaterPlanet", "TerrestrialPlanet", "RockyPlanet", "GasPlanet", "IcyPlanet", "MoltenPlanet" };
        public const double MinMass = 1.5912E+29, MaxMass = 2.9835E+32;
        public const double MinSize = 1_000_000, MaxSize = 10_000_000;
        public static string[] StarNames = null;
        public double trueRadius;
        public PlanetRenderer[] planets;

        public Color starColor;

        public override void Create(Vector2 position)
        {
            StarNames ??= Resources.Load<TextAsset>("StarNames").text.Split('\n');

            Position = position;
            Name = StarNames[RandomNum.Get(0, StarNames.Length, RandomNumberGenerator)].Trim() + " " + RandomNum.GetString(1, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);

            trueRadius = RandomNum.CurveAt(RandomNum.GetInfiniteDouble(0.4, RandomNumberGenerator), 4, 1.2) * 3;
            Radius = trueRadius * 1_000_000; //RandomNum.Get(MinSize, MaxSize, RandomNumberGenerator);
            Temperature = RandomNum.Get(3000, 10000, RandomNumberGenerator);
        }

        public void SpawnPlanets(Transform transform)
        {
            planets = new PlanetRenderer[RandomNumberGenerator.Next(0, 5)];

            for (int i = 0; i < planets.Length; i++)
            {
                string planetName = PlanetStrings[RandomNumberGenerator.Next(0, PlanetStrings.Length)];
                PlanetRenderer newPlanet = Object.Instantiate(Resources.Load<PlanetRenderer>("Objects/Planet/" + planetName), transform);
                newPlanet.Spawn(Vector2.zero, RandomNumberGenerator.Next());
                newPlanet.Scale = newPlanet.Scale / (float)trueRadius;

                float distance = RandomNum.GetFloat(1, 4, newPlanet.Target.RandomNumberGenerator);
                float currentRot = RandomNum.GetFloat(0, Mathf.PI * 2, newPlanet.Target.RandomNumberGenerator);

                newPlanet.Target.Position = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot)) * distance;

                planets[i] = newPlanet;
                (newPlanet.Target as Planet).sun = this;
            }
        }

        public override string GetBonusTypes() =>
            "Temperature - " + Temperature;
    }
}
