using UnityEngine;

namespace Universe.CelestialBodies
{
    public class BlackHoleAccretionDisk : CelestialBody
    {
        public override string TypeString => "Asteroid";

        public override string TravelTarget => "RockyPlanet";

        public override bool Circular => true;

        public BlackHole blackHole;
        public bool IsSupermassive;
        public float rotationOffset;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Asteroid";
            Radius = 10;
            Mass = 1000;
            rotationOffset = RandomNum.GetFloat(360, RandomNumberGenerator);
        }

        public void Create(Vector2 pos, BlackHole black)
        {
            Position = pos;
            Name = "Asteroid";
            Radius = 10;
            Mass = 1000;
            rotationOffset = RandomNum.GetFloat(360, RandomNumberGenerator);
            blackHole = black;
        }

        public override string GetBonusTypes()
        {
            return "Black Hole - " + blackHole.Name;
        }
    }
}
