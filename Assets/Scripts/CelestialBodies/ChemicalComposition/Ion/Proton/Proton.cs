using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class Proton : CelestialBody
    {
        public override string TypeString => "Proton";

        public override string TravelTarget => "Proton";

        public override bool Circular => true;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = 1e-15 * Measurement.M;
            Name = "Proton";
        }
    }
}
