using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class Electron : CelestialBody
    {
        public override string TypeString => IsAntimatter ? "Positron" : "Electron";

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public bool IsAntimatter = false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = 2.82e-15 * Measurement.M;
            Name = "Electron";
        }

        public void Antimatter()
        {
            Name = "Positron";
            IsAntimatter = true;
        }
    }
}
