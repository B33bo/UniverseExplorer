using System.Collections.Generic;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Universe : CelestialBody
    {
        private static UniverseInfo DefualtInfo = new UniverseInfo(DefaultUniverseColorer.Instance);
        public static UniverseInfo LoadedInfo
        {
            get
            {
                if (BodyManager.Universe != null)
                    return BodyManager.Universe.universeInfo;
                return DefualtInfo;
            }
        }

        public Color foreground, background;
        public double RotationSpeed;
        public override string TypeString { get => "Universe"; }
        public UniverseInfo universeInfo;
        public int Points;
        public bool Star;

        public override void Create(Vector2 position)
        {
            Position = position;

            Radius = double.MaxValue;

            foreground = new Color(1, .368f, .776f);
            background = new Color(0, .337f, .600f);

            RotationSpeed = RandomNum.Get(360f, RandomNumberGenerator);

            Points = RandomNum.Get(3, 10, RandomNumberGenerator);

            universeInfo = new UniverseInfo(DefaultUniverseColorer.Instance.BlackBodyRadiation);

            Name = (int)Position.x + ":" + (int)Position.y + ":" + Seed; // Must come last
            // Comes last because if nothing uses the RandomNumberGenerator property, and the seed hasn't been initialised, then
            // seed it set to 0 by default. Getting the seed does not initialise it so it just says 0
            // by referencing RandonNumberGenerator, we are forcing the seed to initialise
        }

        private Gradient RandomGradient()
        {
            List<(float, Color)> keys = new List<(float, Color)>();

            for (float val = 0; val < 1; val += RandomNum.GetFloat(.75f, RandomNumberGenerator))
            {
                if (keys.Count >= 8)
                    break;
                if (val >= 1)
                    val = 1;
                keys.Add((val, RandomNum.GetColor(RandomNumberGenerator)));
            }

            Gradient g = new()
            {
                colorKeys = new GradientColorKey[keys.Count],
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) },
            };

            GradientColorKey[] colorKeys = new GradientColorKey[keys.Count];
            for (int i = 0; i < keys.Count; i++)
                colorKeys[i] = new GradientColorKey(keys[i].Item2, keys[i].Item1);
            g.colorKeys = colorKeys;

            Debug.Log(g.colorKeys[0].color);
            return g;
        }

        public override string TravelTarget => "Universe";
        public override bool Circular => true;

        public struct UniverseInfo
        {
            public Gradient BlackBodyRadiation;

            public UniverseInfo(DefaultUniverseColorer defaultUniverseColorer)
            {
                BlackBodyRadiation = defaultUniverseColorer.BlackBodyRadiation;
            }

            public UniverseInfo(Gradient blackBody)
            {
                BlackBodyRadiation = blackBody;
            }
        }
    }
}
