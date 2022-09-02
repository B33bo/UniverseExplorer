using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class Compound : CelestialBody
    {
        public Chemical chemical;

        public override string TypeString => "Compound";

        public override string TravelTarget => string.Empty;

        public override bool Circular => true;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            RandomNum.Init(RandomNumberGenerator);
        }

        public void Create(Vector2 pos, Chemical chemical)
        {
            Position = pos;
            Name = chemical.ToString();
            RandomNum.Init(RandomNumberGenerator);
        }
    }
}
