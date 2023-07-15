using UnityEngine;

namespace Universe.Blocks
{
    public class BasicBlock : Block
    {
        private string typeString;
        private string travelTarget;
        public override string TypeString => typeString;

        public override string TravelTarget => travelTarget;

        public override void Create(Vector2 pos)
        {
            Create(pos, "Unknown", "");
        }

        public void Create(Vector2 pos, string blockName, string travelTarget)
        {
            Position = pos;
            typeString = blockName;
            Name = typeString;
            this.travelTarget = travelTarget;
            Width = 1 * Measurement.M;
            Height = 1 * Measurement.M;
        }
    }
}
