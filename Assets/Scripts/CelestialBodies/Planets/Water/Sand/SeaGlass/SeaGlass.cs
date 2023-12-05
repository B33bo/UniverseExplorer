using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class SeaGlass : CelestialBody
    {
        public override string TypeString => "Seaglass";

        public override string TravelTarget => "";

        public override bool Circular => false;

        [InspectableVar("Points", Params = new object[] { 4, 12 })]
        public int Points = 0;

        [InspectableVar("Smoothness", Params = new object[] { 3, 6 })]
        public int Smoothness = 1;

        [InspectableVar("Shape", Params = new object[] { 0f, .2f })]
        public float Shape = 1;

        private Type _type;

        [InspectableVar("Type")]
        public Type SeaglassType { get => _type; set { _type = value; color = TypeColor[(int)value]; } }

        [InspectableVar("Color")]
        public Color color;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Seaglass";
            Points = RandomNum.Get(4, 12, RandomNumberGenerator);
            Smoothness = RandomNum.Get(3, 6, RandomNumberGenerator);
            Shape = RandomNum.GetFloat(0, .2f, RandomNumberGenerator);
            Width = Measurement.cm;
            Height = Measurement.cm;
            SeaglassType = (Type)RandomNum.GetIndexFromWeights(WeightsOfType, RandomNumberGenerator);
        }

        public enum Type
        {
            White,
            Green,
            Brown,
            Blue,
            Cyan,
            Red,
            Yellow
        }

        public static readonly float[] WeightsOfType = new float[]
        {
            200,
            100,
            50,
            1,
            .7f,
            .4f,
            .1f
        };

        public static readonly Color[] TypeColor = new Color[]
        {
            new Color(.76471f, .76471f, .76471f),
            new Color(0, .5f, 0),
            new Color(179 / 255f, 74 / 255f, 0),
            new Color(0, 0, .5f),
            new Color(0, .5f, .5f),
            new Color(.25f, 0, 0),
            new Color(1, 1, .5f),
        };
    }
}
