using UnityEngine;
using System.Collections.Generic;

namespace Universe.CelestialBodies
{
    public class Aurora : CelestialBody
    {
        public const float FlareSize = 0.1f;
        public override string TypeString => "Aurora";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public static readonly float[] WeightChart = new float[]
        {
            100, //Green
            50, //Pink
            10, //Blue
            1, //Red
            0.1f, // Rainbow
        };
        const float sumOfWeightChart = 161.1f;

        public int Flares;
        public Color color;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Aurora " + RandomNum.GetPlanetName(RandomNumberGenerator);
            Width = RandomNum.Get(8, 56, RandomNumberGenerator);
            Height = 5;
            Flares = Mathf.CeilToInt((float)Width / FlareSize);
            Mass = 0;

            float randomWeight = RandomNum.GetFloat(0, sumOfWeightChart, RandomNumberGenerator);

            //fine because the weights correspond to the index of the enum (green = 0)
            color = (Color)RandomNum.GetIndexFromWeight(WeightChart, randomWeight);
        }

        public enum Color
        {
            Green,
            Pink,
            Blue,
            Red,
            Rainbow,
        }
    }
}
