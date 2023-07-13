using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Desert
{
    public class TumbleWeed : CelestialBody
    {
        public override string TypeString => "Tumble Weed";

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public double rotation;

        public override void Create(Vector2 pos)
        {
            var humanNames = Resources.Load<TextAsset>("HumanNames").text.Split('\n');
            Position = pos;
            Radius = RandomNum.Get(0, 4, RandomNumberGenerator) * Measurement.M;

            rotation = RandomNum.Get(0, 360f, RandomNumberGenerator);

            Name = humanNames[RandomNum.Get(19480, 20423, RandomNumberGenerator)].Trim() + " The Tumbleweed";
        }
    }
}
