﻿using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class WaterPlanet : Planet
    {
        public const double MinScale = 1000, MaxScale = 12000;
        public const double MinMass = 3e22, MaxMass = 4e24;

        public override string TypeString => "Water Planet";

        public override string PlanetTargetScene => "WaterPlanet";

        public override bool Circular => true;

        public override string ObjectFilePos => "Planets/Water";

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
            Name = GenerateName();
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);

            if (RandomNum.GetBool(RandomNumberGenerator))
            {
                float r = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                float g = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                float b = RandomNum.GetFloat(-.05f, .05f, RandomNumberGenerator);
                waterColor += new Color(r, g, b);
            }
            else
            {
                waterColor = RandomNum.GetColor(RandomNumberGenerator);
            }
        }
    }
}
