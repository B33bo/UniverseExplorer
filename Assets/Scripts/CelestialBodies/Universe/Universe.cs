using System;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Universe : CelestialBody
    {
        public Color foreground, background;
        public double RotationSpeed;
        public override string TypeString { get => "Universe"; }

        public override void Create(Vector2 position)
        {
            Position = position;

            Radius = double.MaxValue;

            foreground = new Color(1, .368f, .776f);
            background = new Color(0, .337f, .600f);

            RotationSpeed = RandomNum.Get(360f, RandomNumberGenerator);

            Name = (int)Position.x + ":" + (int)Position.y + ":" + Seed; //Must come last
            //Comes last because if nothing uses the RandomNumberGenerator property, and the seed hasn't been initialised, then
            //seed it set to 0 by default. Getting the seed does not initialise it so it just says 0
            //by referencing RandonNumberGenerator, we are forcing the seed to initialise
        }

        public override string TravelTarget => "Universe";
        public override bool Circular => true;
    }
}
