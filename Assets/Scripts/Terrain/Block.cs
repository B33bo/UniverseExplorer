using UnityEngine;

namespace Universe
{
    public abstract class Block : CelestialBody
    {
        public override bool Circular => false;
    }
}
