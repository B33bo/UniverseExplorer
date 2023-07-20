using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class Star : CelestialBody
    {
        public const int Mercury = 0, Venus = 1, Earth = 2, Mars = 3, Jupiter = 4, Saturn = 5, Uranus = 6, Neptune = 7, Pluto = 8;
        public const float minTemp = 3000, maxTemp = 10_000;
        public override bool Circular => true;
        public override string TravelTarget => "Star";
        public override string TypeString => "Star";
        private double temperature;

        private static readonly string[] PlanetStrings = new string[] { "WaterPlanet", "TerrestrialPlanet", "RockyPlanet", "GasPlanet", "IcyPlanet", "MoltenPlanet" };
        public const double MinMass = 1.5912E+29, MaxMass = 2.9835E+32;
        public const double MinSize = 1_000_000, MaxSize = 10_000_000;
        public static string[] StarNames = null;
        public double trueRadius;
        public PlanetRenderer[] planets;
        public System.Action<object> ColorChange;
        private Color _color;

        [InspectableVar("Color")]
        public Color StarColor { get => _color; set
            {
                _color = value;
                ColorChange(value);
            } 
        }

        [InspectableVar("Temperature", Params = new object[] { 3000, 10_000 })]
        public double Temperature
        {
            get => temperature;
            set
            {
                temperature = value;
                ColorChange(value);
            }
        }

        public override void Create(Vector2 position)
        {
            Position = position;

            if (Seed == 0)
            {
                Name = "Sol";
                Mass = 1.989e30 * Measurement.Kg;
                Radius = 696_340 * Measurement.Km;
                trueRadius = 3.96340 * Measurement.Km;
                temperature = 5600;
                return;
            }

            Name = RandomNum.GetPlanetName(Seed);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);

            trueRadius = RandomNum.CurveAt(RandomNum.GetInfiniteDouble(0.4, RandomNumberGenerator), 4, 1.2) * 3;
            Radius = trueRadius * 1_000_000;
            temperature = RandomNum.Get(3000, 10000, RandomNumberGenerator);
        }

        public void SpawnPlanets(Transform transform)
        {
            if (Seed == 0)
            {
                SpawnEarthSolarSystem(transform);
                return;
            }

            planets = new PlanetRenderer[RandomNumberGenerator.Next(0, 5)];

            for (int i = 0; i < planets.Length; i++)
            {
                string planetName = PlanetStrings[RandomNumberGenerator.Next(0, PlanetStrings.Length)];
                PlanetRenderer newPlanet = Object.Instantiate(Resources.Load<PlanetRenderer>("Objects/Planet/" + planetName), transform);
                newPlanet.Spawn(Vector2.zero, RandomNumberGenerator.Next());
                newPlanet.Scale /= (float)trueRadius;

                float distance = RandomNum.GetFloat(1, 4, newPlanet.Target.RandomNumberGenerator);
                float currentRot = RandomNum.GetFloat(0, Mathf.PI * 2, newPlanet.Target.RandomNumberGenerator);

                newPlanet.Target.Position = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot)) * distance;

                planets[i] = newPlanet;
                (newPlanet.Target as Planet).sun = this;
            }
        }

        private void SpawnEarthSolarSystem(Transform transform) // idfk what our solar system is called, why doesn't it have a name
        {
            var data = CSVreader.GetValues<PlanetCSVData>(Resources.Load<TextAsset>("SolarSystem").text.Trim('\r').Split('\n'));
            planets = new PlanetRenderer[8];
            float dist = 0;

            for (int i = 0; i < planets.Length; i++)
            {
                PlanetRenderer newPlanet = Object.Instantiate(Resources.Load<PlanetRenderer>("Objects/Planet/" + data[i].PlanetType), transform);
                newPlanet.Spawn(Vector2.zero, i);
                newPlanet.Scale /= (float)trueRadius;
                newPlanet.Scale *= data[i].Multiplier * 1.2f;

                dist += RandomNum.GetFloat(2, 4, newPlanet.Target.RandomNumberGenerator);

                float currentRot = RandomNum.GetFloat(0, Mathf.PI * 2, newPlanet.Target.RandomNumberGenerator);

                newPlanet.Target.Position = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot)) * dist;

                planets[i] = newPlanet;
                (newPlanet.Target as Planet).sun = this;
            }
        }

        public override string GetBonusTypes() =>
            "Temperature - " + temperature;

        private struct PlanetCSVData
        {
            public string Name, PlanetType;
            public float Multiplier;
        }
    }
}
