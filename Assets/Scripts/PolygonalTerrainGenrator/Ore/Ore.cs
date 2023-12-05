using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Ore : CelestialBody
    {
        public override string TypeString => oreType + " Ore";

        public override string TravelTarget => travelTarget;

        public override bool Circular => false;

        private string oreType;
        private string travelTarget;
        public Color OreColor;

        public override void Create(Vector2 pos)
        {
            Create(pos, "Unknowninium", "", Color.magenta);
        }

        public void Create(Vector2 pos, string type, string travelTarget, Color oreColor)
        {
            Name = type;
            Position = pos;
            oreType = type;
            this.travelTarget = travelTarget;
            OreColor = oreColor;

            Width = RandomNum.Get(1 * Measurement.cm, 200 * Measurement.cm, RandomNumberGenerator);
            Height = RandomNum.Get(.5f, 2f, RandomNumberGenerator) * Width;
        }
    }
}
