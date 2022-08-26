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

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Asteroid";
            Radius = 10;
            Mass = 1000;
        }

        public void Create(Vector2 pos, BlackHole black)
        {
            Position = pos;
            Name = "Asteroid";
            Radius = 10;
            Mass = 1000;
            blackHole = black;
        }

        public override string GetBonusTypes()
        {
            return "Black Hole - " + blackHole.Name;
        }
    }
}
