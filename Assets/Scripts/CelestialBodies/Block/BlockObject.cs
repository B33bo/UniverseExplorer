using UnityEngine;

namespace Universe
{
    public class BlockObject : CelestialBody
    {
        public string blockTypeString, blockTravelTarget;

        public override string TypeString => blockTypeString;

        public override string TravelTarget => blockTravelTarget;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
        }
    }
}
