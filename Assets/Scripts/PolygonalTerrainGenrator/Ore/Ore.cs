using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Ore : CelestialBody
    {
        public override string TypeString => oreType + " Ore";

        public override string TravelTarget => travelTarget;

        public override bool Circular => false;

        private string oreType;
        private string travelTarget;
        public Color OreColor;

        public void LoadRandomOre()
        {
            float weightIndex = RandomNum.GetFloat(OreGenerator.weightTotal, RandomNumberGenerator);
            int index = RandomNum.GetIndexFromWeights(OreGenerator.weights, weightIndex);
            Load(OreGenerator.ores[index]);
        }

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Width = RandomNum.Get(1 * Measurement.cm, 200 * Measurement.cm, RandomNumberGenerator);
            Height = RandomNum.Get(.5f, 2f, RandomNumberGenerator) * Width;
        }

        public void Load(Ore ore)
        {
            Load(ore.oreType, ore.travelTarget, ore.OreColor);
        }

        public void Load(string type, string travelTarget, Color oreColor)
        {
            Name = type;
            oreType = type;
            this.travelTarget = travelTarget;
            OreColor = oreColor;
        }
    }
}
