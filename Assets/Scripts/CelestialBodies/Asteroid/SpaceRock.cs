using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class SpaceRock : Planet
    {
        public override string TypeString => type.ToString();

        public override bool Circular => false;

        public override string ObjectFilePos => "Objects/Asteroid";

        public override string TravelTarget => PlanetTargetScene;

        public override string PlanetTargetScene => "RockyPlanet";

        public Type type;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = RandomNum.Get(0, 999, RandomNumberGenerator) + " " + RandomNum.GetPlanetName(RandomNumberGenerator);
            type = Type.Asteroid;
        }

        public enum Type
        {
            Asteroid, //Rocks
            Comet, //Icy
            Meteoroid, //Tiny asteroid
        }
    }
}
