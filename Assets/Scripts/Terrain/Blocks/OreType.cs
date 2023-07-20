using UnityEngine;

namespace Universe.Blocks
{
    public struct OreType
    {
        public static readonly OreType Unknownium = new OreType("Unknownium", new Color(1, 0, 1));
        public static readonly OreType[] Ores = new OreType[]
        {
            new OreType("None", Color.clear),
            new OreType("Diamond", new Color(0, 199f / 255, 1)),
            new OreType("Ruby", new Color(1, 0, 62f / 255)),
            new OreType("Sapphire", new Color(0, 100f / 255, 1)),
            new OreType("Gold", new Color(1, 216f / 255, 0)),
            new OreType("Silver", new Color(.81f, .81f, .81f)),
            new OreType("Bronze", new Color(194f / 255, 93f / 255, 18f / 255)),
            new OreType("Emerald", new Color(0, 194f / 255, 0)),
            new OreType("Copper", new Color(76f / 255, 239f / 255, 169f / 255)),
            new OreType("Rainbonite"),
            Unknownium,
        };

        public enum EnumNames
        {
            None,
            Diamond,
            Ruby,
            Sapphire,
            Gold,
            Silver,
            Bronze,
            Emerald,
            Copper,
            Rainbonite,
            Unknownium,

            Max = Unknownium,
        }

        public string Name { get; private set; }
        public Color Color { get; private set; }

        OreType(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        OreType(string name)
        {
            Name = name;
            Color = Color.black;
        }
    }
}
