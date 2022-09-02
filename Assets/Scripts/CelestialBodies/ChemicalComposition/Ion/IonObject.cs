using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class IonObject : CelestialBody
    {
        public Ion ion;
        public bool IsAbstract;

        public override string TypeString => ion.Charge == 0 ? "Atom" : "Ion";

        public override string TravelTarget => "Ion";

        public override bool Circular => true;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = ion.Element;
            RandomNum.Init(RandomNumberGenerator);
        }

        public void Create(Vector2 pos, Ion ion)
        {
            this.ion = ion;
            Create(pos);
            RandomNum.Init(RandomNumberGenerator);
        }
    }
}
