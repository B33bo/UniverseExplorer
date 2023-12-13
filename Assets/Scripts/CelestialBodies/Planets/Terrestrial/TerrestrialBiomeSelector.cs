using UnityEngine;
using UnityEngine.UIElements;
using Universe.CelestialBodies.Planets;

namespace Universe.Terrain
{
    public class TerrestrialBiomeSelector : BiomeManager
    {
        [SerializeField]
        private BiomeTemperatureType cold, mild, hot;

        private float waterLevel = .3f;

        private float seedYPos;

        private void Awake()
        {
            seedYPos = (float)(new System.Random(BodyManager.GetSeed()).NextDouble() * 20_000 - 10_000);

            if (BodyManager.Parent is TerrestrialPlanet terrestrialPlanet)
                waterLevel = terrestrialPlanet.waterLevel;
        }

        public Temperature GetTemperatureAt(float x)
        {
            const float temperatureChangeValues = 1500;

            float xPosition = Mathf.Floor(x / temperatureChangeValues) * temperatureChangeValues;

            double temperature = new System.Random(new Vector2(xPosition, seedYPos).HashPos(BodyManager.GetSeed())).NextDouble();

            if (temperature > .7f)
                return Temperature.Hot;
            if (temperature < .3f)
                return Temperature.Cold;
            return Temperature.Mild;
        }

        const float waterChangeVal = 200;
        private bool IsWater(float x)
        {
            x = Mathf.Floor(x / waterChangeVal) * waterChangeVal;
            int seed = new Vector2(x, seedYPos).HashPos(BodyManager.GetSeed());
            return new System.Random(seed).NextDouble() < waterLevel;
        }

        private bool IsBeach(float x)
        {
            x = Mathf.Floor(x / waterChangeVal) * waterChangeVal;
            int seed = new Vector2(x, seedYPos).HashPos(BodyManager.GetSeed());
            if (new System.Random(seed).NextDouble() < .25f)
                return false;

            return IsWater(x + waterChangeVal) || IsWater(x - waterChangeVal);
        }

        public override PolyTerrainLayer GetBiomeAt(float x)
        {
            BiomeTemperatureType biomeType = GetTemperatureAt(x) switch
            {
                Temperature.Cold => cold,
                Temperature.Mild => mild,
                Temperature.Hot => hot,
                _ => null,
            };

            if (biomeType.UseSea && IsWater(x))
                return biomeType.Sea;

            if (biomeType.UseBeach && IsBeach(x))
                return biomeType.Beach;

            return biomeType.Normal[RandomNum.GetIndexFromWeights(biomeType.Weights, GetSeedForPosition(x))];
        }

        public enum Temperature
        {
            Cold,
            Mild,
            Hot,
        }
    }
}
