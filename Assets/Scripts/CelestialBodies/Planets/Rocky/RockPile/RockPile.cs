using UnityEngine;

namespace Universe.CelestialBodies.Planets.Rocky
{
    public class RockPile : CelestialBody
    {
        public override string TypeString => "Rock Pile";

        public override string TravelTarget => "RockPile";

        public override bool Circular => false;

        public int Rocks;
        private Mesh[] RockMeshs;

        public Color[] colors;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Rocks = RandomNum.Get(4, 8, RandomNumberGenerator);
            RockMeshs = new Mesh[Rocks];
            colors = new Color[Rocks];

            for (int i = 0; i < Rocks; i++)
            {
                RockMeshs[i] = Boulder.GetRock(RandomNumberGenerator);
            }

            Width = GetWidth(RockMeshs[0].vertices) * 0.001;
            Mass = 5 * Rocks;
            Height = Rocks * 0.001;
            Name = $"Rock Pile ({Rocks})";
        }

        private float GetWidth(Vector3[] verticies)
        {
            float x = 0;
            for (int i = 0; i < verticies.Length; i++)
            {
                if (x < verticies[i].x)
                    x = verticies[i].x;
            }
            return x * 2;
        }

        public Mesh this[int index]
        {
            get => RockMeshs[index];
        }

        public override string GetBonusTypes()
        {
            return "Rocks - " + Rocks;
        }
    }
}
