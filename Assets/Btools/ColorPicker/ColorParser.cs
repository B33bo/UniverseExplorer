using UnityEngine;
using System;

namespace Btools.utils
{
    public static class ColorParser
    {
        public static bool TryParse(string s, out Color color)
        {
            color = default;
            if (s.Length == 0)
                return false;

            if (s.StartsWith("#"))
                return TryParseHex(s.Substring(1), out color);
            if (s.Length < 4)
                return false;

            var start = s.ToLower().Substring(0, 4);
            return start switch
            {
                "hex:" => TryParseHex(s.Substring(4), out color),
                "num:" => TryParseNum(s.Substring(4), out color),
                "rgb:" => TryParseRGB(s.Substring(4), out color),
                "hsv:" => TryParseHSV(s.Substring(4), out color),
                "rng:" => GetRandomColor(s.Substring(4), out color),
                _ => TryParseHex(s, out color),
            };
        }

        private static bool TryParseHex(string s, out Color color)
        {
            color = default;

            if (s.Length != 6 && s.Length != 8)
                return false;
            try
            {
                var r = Convert.ToByte(s[0].ToString() + s[1], 16);
                var g = Convert.ToByte(s[2].ToString() + s[3], 16);
                var b = Convert.ToByte(s[4].ToString() + s[5], 16);
                var a = byte.MaxValue;

                if (s.Length == 8)
                    a = Convert.ToByte(s[6].ToString() + s[7], 16);
                color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool TryParseNum(string s, out Color color)
        {
            color = default;
            if (!uint.TryParse(s, out uint num))
                return false;

            //RGBA
            byte r = (byte)(num >> 24);
            byte g = (byte)((num << 8) >> 24);
            byte b = (byte)((num << 16) >> 24);
            byte a = (byte)num;

            color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            return true;
        }

        private static bool TryParseRGB(string s, out Color color)
        {
            var components = s.Split(',');
            color = default;

            if (components.Length != 3 && components.Length != 4)
                return false;

            if (!byte.TryParse(components[0], out byte r))
                return false;
            if (!byte.TryParse(components[1], out byte g))
                return false;
            if (!byte.TryParse(components[2], out byte b))
                return false;

            color.r = r / 255f;
            color.g = g / 255f;
            color.b = b / 255f;
            color.a = 1;

            if (components.Length == 4)
            {
                if (!byte.TryParse(components[3], out byte a))
                    return false;
                color.a = a / 255f;
            }

            return true;
        }

        private static bool TryParseHSV(string str, out Color color)
        {
            var components = str.Split(',');
            color = default;

            if (components.Length != 3 && components.Length != 4)
                return false;

            if (!ushort.TryParse(components[0], out ushort h))
                return false;
            if (!byte.TryParse(components[1], out byte s))
                return false;
            if (!byte.TryParse(components[2], out byte v))
                return false;

            if (h > 360 || s > 100 || v > 100)
                return false;
            float h_float = h / 360f;
            float s_float = s / 100f;
            float v_float = v / 100f;

            color = Color.HSVToRGB(h_float, s_float, v_float);
            color.a = 1;

            if (components.Length == 4)
            {
                if (!byte.TryParse(components[3], out byte a))
                    return false;
                if (a > 100)
                    return false;
                color.a = a / 100f;
            }

            return true;
        }

        private static bool GetRandomColor(string s, out Color color)
        {
            s = s.ToLower();
            color = new Color(1, 1, 1, 1);
            if (s.Contains("r"))
                color.r = UnityEngine.Random.value;

            if (s.Contains("g"))
                color.g = UnityEngine.Random.value;

            if (s.Contains("b"))
                color.b = UnityEngine.Random.value;

            if (s.Contains("a"))
                color.a = UnityEngine.Random.value;

            return true;
        }
    }
}
