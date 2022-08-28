using UnityEngine;

namespace Universe.CelestialBodies.Terrain
{
    public class SandGround : CelestialBody
    {
        public override string TypeString => "Sand";

        public override string TravelTarget => "SandGround";

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Sand";
        }
    }
}
