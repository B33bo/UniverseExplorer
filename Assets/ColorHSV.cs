using UnityEngine;

namespace Universe
{
    public struct ColorHSV
    {
        public float _h, _s, _v, _a;

        public float h
        {
            get => _h; set
            {
                _h = value % 1;
                if (_h < 0)
                    _h += 1;
            }
        }
        public float s { get => _s; set => _s = Mathf.Clamp(value, 0, 1); }
        public float v { get => _v; set => _v = Mathf.Clamp(value, 0, 1); }
        public float a { get => _a; set => _a = Mathf.Clamp(value, 0, 1); }

        public ColorHSV(float H, float S, float V)
        {
            _h = H;
            _s = S;
            _v = V;
            _a = 1;
        }

        public ColorHSV(float H, float S, float V, float A)
        {
            _h = H;
            _s = S;
            _v = V;
            _a = A;
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
    }
}
