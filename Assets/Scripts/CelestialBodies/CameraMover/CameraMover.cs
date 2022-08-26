using UnityEngine;

namespace Universe.CelestialBodies
{
    public class CameraMover : CelestialBody
    {
        public override string TypeString => "Camera Mover";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = pos.ToString();
        }

        public override string GetBonusTypes()
        {
            return "used to move the camera, only for implementation's sake";
        }
    }
}
