using UnityEngine;

namespace Universe.Blocks
{
    public class Wave : Block
    {
        public override string TypeString => "Water";

        public override string TravelTarget => "";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Water";
            Width = Measurement.M;
        }
    }
}
