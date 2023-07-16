using System.Text;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Nebula : CelestialBody
    {
        public Band[] Bands;

        public override string TypeString => "Nebula";

        public override string TravelTarget => "Lights";

        public override bool Circular => false;
        private Vector2 bottomLeft, topRight, averagePos;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = RandomNum.GetPlanetName(RandomNumberGenerator) + " Nebula";

            Bands = new Band[RandomNum.Get(1, 5, RandomNumberGenerator)];
            Mass = 0;

            Vector2 sumPositions = Vector2.zero;
            int positionCount = 0;

            for (int i = 0; i < Bands.Length; i++)
            {
                Bands[i] = GenerateBand(ref sumPositions);
                positionCount += Bands[i].lights.Length;
            }

            averagePos = sumPositions / positionCount;

            Mass *= Measurement.Kg * 10000;

            Width = (topRight.x - bottomLeft.x) * Measurement.LY;
            Height = (topRight.y - bottomLeft.y) * Measurement.LY;
            Center();
        }

        public Band GenerateBand(ref Vector2 sumPositions)
        {
            Band b = new Band()
            {
                color = RandomNum.GetColor(RandomNumberGenerator),
                lights = new LightInfo[RandomNum.Get(3, 7, RandomNumberGenerator)],
            };
            const float minRadius = 1, maxRadius = 3;

            float rotation = RandomNum.GetFloat(0, 360, RandomNumberGenerator);
            Vector2 startPos = RandomNum.GetVector(2, RandomNumberGenerator) - Vector2.one;

            b.lights[0] = new LightInfo()
            {
                position = startPos,
                radius = RandomNum.GetFloat(minRadius, maxRadius, RandomNumberGenerator),
            };
            sumPositions += startPos;

            for (int i = 1; i < b.lights.Length; i++)
            {
                Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));

                float radius = RandomNum.GetFloat(minRadius, maxRadius, RandomNumberGenerator);
                float squash = RandomNum.GetFloat(radius * 0.75f, radius, RandomNumberGenerator);

                Mass += radius;
                b.lights[i] = new LightInfo()
                {
                    position = b.lights[i - 1].position + direction * (radius + b.lights[i - 1].radius - squash),
                    radius = radius,
                };

                sumPositions += b.lights[i].position;
                rotation += RandomNum.GetFloat(-25, 25, RandomNumberGenerator);

                if (b.lights[i].position.x < bottomLeft.x)
                    bottomLeft.x = b.lights[i].position.x;
                else if (b.lights[i].position.x > topRight.x)
                    topRight.x = b.lights[i].position.x;

                if (b.lights[i].position.y < bottomLeft.y)
                    bottomLeft.y = b.lights[i].position.y;
                else if (b.lights[i].position.y > topRight.y)
                    topRight.y = b.lights[i].position.y;
            }

            return b;
        }

        public void Center()
        {
            for (int i = 0; i < Bands.Length; i++)
            {
                for (int j = 0; j < Bands[i].lights.Length; j++)
                    Bands[i].lights[j].position -= averagePos;
            }
        }

        public struct Band
        {
            public Color color;
            public LightInfo[] lights;
        }

        public struct LightInfo
        {
            public Vector2 position;
            public float radius;
        }

        public override string GetBonusTypes()
        {
            StringBuilder colors = new StringBuilder("Colors - ");

            for (int i = 0; i < Bands.Length; i++)
            {
                if (Bands[i].color == Color.black)
                    colors.Append("Rainbow, ");
                else
                    colors.Append(Bands[i].color.ToHumanString() + ", ");
            }

            colors.Remove(colors.Length - 2, 2); // ", "
            return colors.ToString();
        }
    }
}
