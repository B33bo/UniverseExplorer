using UnityEngine;

namespace Universe.Weather
{
    public class Weather : CelestialBody
    {
        public override string TypeString => PrecipitationType.ToString();

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public float PrecipitationIntensity;
        public Precipitation PrecipitationType;

        public float WindDirection;
        public float WindIntensity;

        public float RainbowIntensity;
        public float CloudIntensity;

        public void WeatherChange(WeatherManager weatherTemplate, int weatherIndex)
        {
            // seed based on x position, time, and parent seed
            int weatherSeed = new Vector2(Position.x, weatherIndex).HashPos(Seed);

            System.Random rand = new System.Random(weatherSeed);

            int precipitationIndex = GetPrecipitationType(weatherTemplate, rand);
            PrecipitationType = weatherTemplate.Precipitations[precipitationIndex].Type;
            PrecipitationIntensity = GetIntensityGivenChance(weatherTemplate.Precipitations[precipitationIndex].chance, rand);

            WindDirection = GetIntensity(weatherTemplate.WindDirection, rand);
            WindIntensity = GetIntensity(weatherTemplate.WindIntensity, rand);
            RainbowIntensity = GetIntensity(weatherTemplate.Rainbow, rand);
            CloudIntensity = GetIntensity(weatherTemplate.Clouds, rand);
        }

        private float GetIntensity(WeatherManager.Chance chance, System.Random rand)
        {
            if (rand.NextDouble() > chance.Probability)
                return 0; // didn't happen.

            return GetIntensityGivenChance(chance, rand);
        }

        private float GetIntensityGivenChance(WeatherManager.Chance chance, System.Random rand)
        {
            return chance.Average + RandomNum.GetFloat(-chance.Deviation, chance.Deviation, rand);
        }

        private int GetPrecipitationType(WeatherManager template, System.Random rand)
        {
            float[] weights = new float[template.Precipitations.Length];
            float total = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = template.Precipitations[i].chance.Probability;
                total += weights[i];
            }

            int num = RandomNum.GetIndexFromWeights(weights, (float)rand.NextDouble() * total);
            return num;
        }

        public override void Create(Vector2 pos)
        {
            Position = pos;
        }
    }
}
