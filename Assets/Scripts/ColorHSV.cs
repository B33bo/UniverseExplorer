using UnityEngine;

namespace Universe
{
    public struct ColorHSV
    {
        public float _h, _s, _v, _a;

        public float h { get => _h; set => _h = Mod(value); }
        public float s { get => _s; set => _s = Limit(value); }
        public float v { get => _v; set => _v = Limit(value); }
        public float a { get => _a; set => _a = Limit(value); }

        private static float Mod(float f)
        {
            f %= 1;
            if (f < 0)
                f += 1;
            return f;
        }

        private static float Limit(float f) => Mathf.Clamp01(f);

        public ColorHSV(float H, float S, float V)
        {
            _h = Mod(H);
            _s = Limit(S);
            _v = Limit(V);
            _a = 1;
        }

        public ColorHSV(float H, float S, float V, float A)
        {
            _h = Mod(H);
            _s = Limit(S);
            _v = Limit(V);
            _a = Limit(A);
        }

        private ColorHSV(Color color)
        {
            Color.RGBToHSV(color, out _h, out _s, out _v);
            _a = color.a;
        }

        public static implicit operator ColorHSV(Color c) => new ColorHSV(c);

        public static implicit operator Color(ColorHSV colorHSV)
        {
            Color c = Color.HSVToRGB(colorHSV.h, colorHSV.s, colorHSV.v, false);
            c.a = colorHSV.a;
            return c;
        }

        public static ColorHSV operator +(ColorHSV a, ColorHSV b)
        {
            return new(a.h + b.h, a.s + b.s, a.v + b.v);
        }

        public static ColorHSV operator -(ColorHSV a, ColorHSV b)
        {
            return new(a.h - b.h, a.s - b.s, a.v - b.v);
        }
    }
}
