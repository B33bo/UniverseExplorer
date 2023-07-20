using UnityEngine;

namespace Universe
{
    public static class Extensions
    {
        private static readonly (int hue, string name)[] Colors = new (int, string)[]
        {
            (0, "Red"),
            (30, "Orange"),
            (60, "Yellow"),
            (100, "Green"),
            (160, "Green"),
            (180, "Blue"),
            (240, "Cyan"),
            (270, "Purple"),
            (300, "Magenta"),
            (330, "Red"),
        };

        private static readonly (int value, string name)[] Grayscale = new (int, string)[]
        {
            (0, "Black"),
            (75, "Light Gray"),
            (50, "Gray"),
            (25, "Dark Gray"),
            (100, "White"),
        };

        public static string ToHumanString(this Color c)
        {
            return ToHumanString((ColorHSV)c);
        }

        public static string ToHumanString(this ColorHSV c)
        {
            if (c.s < .05f)
                return GetClosest(Grayscale, (int)(c.v * 255));

            string result = GetClosest(Colors, (int)(c.h * 360));
            if (result == "Magenta" && c.s < .5f)
                return "Pink";
            return result;
        }

        private static string GetClosest((int, string)[] values, int value)
        {
            int below = 0, above = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Item1 < value)
                    continue;

                below = i - 1;
                above = i;
                break;
            }

            if (below < 0)
                return values[above].Item2;

            int belowDiff = value - values[below].Item1;
            int aboveDiff = values[above].Item1 - value;

            return belowDiff > aboveDiff ? values[below].Item2 : values[above].Item2;
        }

        public static int HashPos(this Vector2 vector, int seed)
        {
            int hash = -2128831035;
            hash = (hash * 16777619) ^ vector.x.GetHashCode();
            hash = (hash * 16777619) ^ vector.y.GetHashCode();
            hash = (hash * 16777619) ^ seed.GetHashCode();

            if (vector.y < 0)
                hash = int.MaxValue - hash;
            return hash;
        }
    }
}
