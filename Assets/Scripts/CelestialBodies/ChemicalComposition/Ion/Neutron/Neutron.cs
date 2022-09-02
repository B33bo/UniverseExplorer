using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class Neutron : CelestialBody
    {
        public override string TypeString => "Neutron";

        public override string TravelTarget => "Neutron";

        public override bool Circular => true;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = 0.80e-15 * Measurement.M;
            Name = "Neutron";
        }
    }
}
