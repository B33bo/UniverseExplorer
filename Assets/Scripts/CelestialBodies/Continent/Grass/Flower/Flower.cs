using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Grass
{
    public class Flower : CelestialBody
    {
        public override string TypeString => "Flower";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Color color = Color.white;
        public Type type;
        public Vector2 offset;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            type = (Type)RandomNum.Get(0, 12, RandomNumberGenerator);
            offset = RandomNum.GetVector(-.1f, .1f, RandomNumberGenerator);

            if (type == Type.Tulip)
                color = Color.HSVToRGB(RandomNum.GetFloat(1, RandomNumberGenerator), 1, 1);

            Width = 1 * Measurement.cm;
            Height = 10 * Measurement.cm;
            Name = type switch
            {
                Type.None => "Stalk",
                Type.DandelionSeed => "Dandelion",
                Type.LilyOfTheValley => "Lily of the Valley",
                Type.BlueRose => "Blue Rose",
                Type.BubblegumFlower => "Bubblegum Flower",
                _ => type.ToString(),
            };
        }

        public enum Type
        {
            None,
            Daisy,
            Rose,
            Daffodil,
            Sunflower,
            Poppy,
            Tulip,
            Dandelion,
            DandelionSeed,
            LilyOfTheValley,
            BlueRose,
            BubblegumFlower,
        }
    }
}
