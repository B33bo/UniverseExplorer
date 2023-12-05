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
        private static float totalPlanetWeights;

        private static readonly PlanetRenderer[] PlanetPrefabs = Resources.LoadAll<PlanetRenderer>("Objects/Planet");
        public const double MinMass = 1.5912E+29, MaxMass = 2.9835E+32;
        public const double MinSize = 1_000_000, MaxSize = 10_000_000;
        public static string[] StarNames = null;
        public double trueRadius;
        public PlanetRenderer[] planets;

        [InspectableVar("Color")]
        public Color StarColor;

        [InspectableVar("Temperature", Params = new object[] { 1000, 40_000 })]
        public double Temperature;

        public override void Create(Vector2 position)
        {
            if (totalPlanetWeights == 0)
            {
                for (int i = 0; i < PlanetPrefabs.Length; i++)
                    totalPlanetWeights += PlanetPrefabs[i].SpawnWeight;
            }

            Position = position;

            if (Seed == 0)
            {
                Name = "Sol";
                Mass = 1.989e30 * Measurement.Kg;
                Radius = 696_340 * Measurement.Km;
                trueRadius = 3.96340 * Measurement.Km;
                Temperature = 5600;
                return;
            }

            float nameLengthBonus = Mathf.Log10(Mathf.Abs(position.x)) - 3; // Reeeeeally far away objects have longer names because it's scarier

            if (nameLengthBonus < 1)
                nameLengthBonus = 1;

            int nameLength = (int)(RandomNum.Get(1, 3, RandomNumberGenerator) * nameLengthBonus);
            Name = RandomNum.GetWord(nameLength, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);

            trueRadius = RandomNum.CurveAt(RandomNum.GetInfiniteDouble(0.4, RandomNumberGenerator), 4, 1.2) * 3;
            Radius = trueRadius * 1_000_000;
            Temperature = RandomNum.Get(1000, 20000, RandomNumberGenerator);
            ResetColor();
        }

        public void ResetColor()
        {
            StarColor = Mathf.CorrelatedColorTemperatureToRGB((float)Temperature);
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
                PlanetRenderer newPlanet = Object.Instantiate(PlanetPrefabs[GetPlanet()], transform);
                newPlanet.Spawn(Vector2.zero, RandomNumberGenerator.Next());
                newPlanet.Scale /= (float)trueRadius;
                newPlanet.Scale *= 1.2f;

                float distance = RandomNum.GetFloat(1, 4, newPlanet.Target.RandomNumberGenerator);
                float currentRot = RandomNum.GetFloat(0, Mathf.PI * 2, newPlanet.Target.RandomNumberGenerator);

                newPlanet.Target.Position = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot)) * distance;

                planets[i] = newPlanet;
                (newPlanet.Target as Planet).sun = this;
            }
        }

        private int GetPlanet()
        {
            float weightChosen = RandomNum.GetFloat(totalPlanetWeights, RandomNumberGenerator);
            float sum = 0;

            for (int i = 0; i < PlanetPrefabs.Length; i++)
            {
                float next = sum + PlanetPrefabs[i].SpawnWeight;
                if (next > weightChosen)
                    return i;
                sum = next;
            }
            return PlanetPrefabs.Length;
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
            "Temperature - " + Temperature;

        private struct PlanetCSVData
        {
            public string Name, PlanetType;
            public float Multiplier;
        }
    }
}
