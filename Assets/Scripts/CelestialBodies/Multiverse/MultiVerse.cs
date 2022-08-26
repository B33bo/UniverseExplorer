using System;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class MultiVerse : CelestialBody
    {
        private static MultiVerse _multiverse;

        public static MultiVerse Multiverse
        {
            get
            {
                if (_multiverse is null)
                {
                    _multiverse = new MultiVerse();
                    _multiverse.Create(Vector2.zero);
                }
                return _multiverse;
            }
        }

        public override string TypeString => "Multiverse";
        public override string TravelTarget => "Multiverse";
        public override bool Circular => true;

        public override void Create(Vector2 position)
        {
            Position = position;
            Width = double.PositiveInfinity;
            Height = double.PositiveInfinity;
            Radius = double.PositiveInfinity;
        }
    }
}
