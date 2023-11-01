using UnityEngine;

namespace Universe.CelestialBodies
{
    public class TestBody : CelestialBody
    {
        public TestBody(Vector2 position, int seed, string target) : base(position) { Seed = seed; this.target = target; }

        public override string TypeString => "test";

        public string target;
        public override string TravelTarget => target;

        public override bool Circular => false;

        public override void Create(Vector2 position)
        {
            Name = "Test Obj";
            Width = double.NaN;
            Position = position;
        }
    }
}
