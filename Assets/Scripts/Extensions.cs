using UnityEngine;

namespace Universe
{
    public static class Extensions
    {
        private static (int hue, string name)[] Colors = new (int, string)[]
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

        private static (int value, string name)[] Grayscale = new (int, string)[]
        {
            (0, "Black"),
            (75, "Light Gray"),
            (50, "Gray"),
            (25, "Dark Gray"),
            (100, "White"),
        };

        public static string ToHumanString(this Color c)
        {
            Color.RGBToHSV(c, out float Hue, out float S, out float V);

            if (S == 0)
                return GetClosest(Grayscale, (int)(V * 255));

            string result = GetClosest(Colors, (int)(Hue * 360));
            if (result == "Magenta" && S < .5f)
                return "Pink";
            return result;
        }

        private static string GetClosest((int, string)[] values, int value)
        {
            int below = 0, above = 0;

            for (int i = 0; i < Colors.Length; i++)
            {
                if (values[i].Item1 < value)
                    continue;

                below = i - 1;
                above = i;
                break;
            }

            int belowDiff = value - values[below].Item1;
            int aboveDiff = values[above].Item1 - value;

            return belowDiff > aboveDiff ? values[below].Item2 : values[above].Item2;
        }
    }
}
