using UnityEngine;

namespace Universe.Blocks
{
    public class OreBlock : Block
    {
        public override string TypeString => Ore.Name;

        public override string TravelTarget => "";

        public OreType Ore { get; private set; }

        public override void Create(Vector2 pos)
        {
            if (Seed > 0 && Seed < OreType.Ores.Length)
                Create(pos, OreType.Ores[Seed]);
            else
                Create(pos, OreType.Unknownium);
        }

        public void Create(Vector2 pos, OreType oreType)
        {
            if (Seed > 0 && Seed < OreType.Ores.Length)
                oreType = OreType.Ores[Seed];
            Position = pos;
            Name = oreType.Name;
            Width = 1;
            Height = 1;
            Ore = oreType;
        }
    }
}
