using Btools.utils;
using System;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Btools.Components
{
    public class ColorPicker : MonoBehaviour
    {
        const float min = 1 / 255f;

        public enum Type : byte
        {
            RGB,
            HSV,
        }

        private (float r, float g, float b) rgb;
        private (float h, float s, float v) hsv;
        private float alpha;

        [SerializeField]
        [HideInInspector]
        private Type type;

        [SerializeField]
        private GameObject RGBObject;

        [SerializeField]
        private GameObject HSVObject;

        [SerializeField]
        private SliderValue[] RGBSliders;

        [SerializeField]
        private SliderValue[] HSVSliders;

        [SerializeField]
        private SliderValue AlphaSlider;

        [SerializeField]
        private Image RGBButton, HSVButton;

        [SerializeField]
        private Slider2D satvalSlider;

        [SerializeField]
        private Image satvalSliderImage;

        [SerializeField]
        private TMP_InputField hexCode;

        [SerializeField]
        private TextMeshProUGUI hexCodeText;

        [SerializeField]
        private Image hexCodeBackground;

        [Space]
        [SerializeField]
        private UnityEvent<Color> OnValueChanged;

        [System.Serializable]
        private class SliderValue
        {
            public Image with, without;
            public Slider slider;
        }

        private void ResetSlidersRGB()
        {
            var oldType = type;
            type = (Type)255;

            //red
            RGBSliders[0].with.color = new Color(1, rgb.g, rgb.b);
            RGBSliders[0].without.color = new Color(0, rgb.g, rgb.b);
            RGBSliders[0].slider.value = rgb.r;

            //green
            RGBSliders[1].with.color = new Color(rgb.r, 1, rgb.b);
            RGBSliders[1].without.color = new Color(rgb.r, 0, rgb.b);
            RGBSliders[1].slider.value = rgb.g;

            //blue
            RGBSliders[2].with.color = new Color(rgb.r, rgb.g, 1);
            RGBSliders[2].without.color = new Color(rgb.r, rgb.g, 0);
            RGBSliders[2].slider.value = rgb.b;

            AlphaSlider.with.color = new Color(rgb.r, rgb.g, rgb.b);
            AlphaSlider.slider.value = alpha;

            Color.RGBToHSV(new Color(rgb.r, rgb.g, rgb.b), out float h, out float s, out float v);
            if (h == 0)
                h = hsv.h;

            satvalSliderImage.color = Color.HSVToRGB(h, 1, 1);
            satvalSlider.Value = new Vector2(s, v);

            hexCode.text = ColorUtility.ToHtmlStringRGB(new Color(rgb.r, rgb.g, rgb.b));
            hexCodeText.color = new Color(1 - rgb.r, 1 - rgb.g, 1 - rgb.b);
            hexCodeBackground.color = new Color(rgb.r, rgb.g, rgb.b, alpha);
            hsv.h = h;

            type = oldType;
        }

        private void ResetSlidersHSV()
        {
            var oldType = type;
            type = (Type)255;

            //hue
            HSVSliders[0].slider.value = hsv.h;

            //saturation
            HSVSliders[1].with.color = Color.HSVToRGB(hsv.h, 1, hsv.v);
            HSVSliders[1].without.color = Color.HSVToRGB(hsv.h, 0, hsv.v);
            HSVSliders[1].slider.value = hsv.s;

            //value
            HSVSliders[2].with.color = Color.HSVToRGB(hsv.h, hsv.s, 1);
            HSVSliders[2].without.color = Color.HSVToRGB(hsv.h, hsv.s, 0);
            HSVSliders[2].slider.value = hsv.v;

            AlphaSlider.with.color = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
            AlphaSlider.slider.value = alpha;

            satvalSlider.Value = new Vector2(hsv.s, hsv.v);
            var colorRGB = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
            satvalSliderImage.color = Color.HSVToRGB(hsv.h, 1, 1);

            hexCode.text = ColorUtility.ToHtmlStringRGB(colorRGB);
            hexCodeText.color = new Color(1 - colorRGB.r, 1 - colorRGB.g, 1 - colorRGB.b);

            colorRGB.a = alpha;
            hexCodeBackground.color = colorRGB;

            type = oldType;
        }

        private void Set(ref float current, float newValue)
        {
            if (type == (Type)255)
                return;
            current = newValue;
            if (type == Type.RGB)
                ResetSlidersRGB();
            else if (type == Type.HSV)
                ResetSlidersHSV();
            OnValueChanged.Invoke(Color);
        }

        public void SetRed(float value) => Set(ref rgb.r, value);
        public void SetGreen(float value) => Set(ref rgb.g, value);
        public void SetBlue(float value) => Set(ref rgb.b, value);

        public void SetHue(float value) => Set(ref hsv.h, value);
        public void SetSaturation(float value) => Set(ref hsv.s, value);
        public void SetValue(float value) => Set(ref hsv.v, value);

        public void SetAlpha(float value) => Set(ref alpha, value);

        public void SwitchToRGB()
        {
            if (type == Type.RGB)
                return;
            type = Type.RGB;
            var color = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
            (rgb.r, rgb.g, rgb.b) = (color.r, color.g, color.b);

            RGBObject.SetActive(true);
            HSVObject.SetActive(false);

            RGBButton.color = new Color(1, 1, 0);
            HSVButton.color = new Color(1, 1, 1);

            ResetSlidersRGB();
        }

        public void SwitchToHSV()
        {
            if (type == Type.HSV)
                return;
            type = Type.HSV;
            var color = new Color(rgb.r, rgb.g, rgb.b);
            Color.RGBToHSV(color, out hsv.h, out hsv.s, out hsv.v);

            RGBObject.SetActive(false);
            HSVObject.SetActive(true);

            RGBButton.color = new Color(1, 1, 1);
            HSVButton.color = new Color(1, 1, 0);

            ResetSlidersHSV();
        }

        public void SetHSV(Vector2 square)
        {
            if (type == Type.RGB)
            {
                Color.RGBToHSV(new Color(rgb.r, rgb.g, rgb.b), out float H, out _, out _);
                bool hueUnknown = H == 0;

                if (!hueUnknown)
                    hsv.h = H;

                if (hueUnknown)
                    H = hsv.h;

                var rgbColor = Color.HSVToRGB(H, square.x, square.y);

                rgb = (rgbColor.r, rgbColor.g, rgbColor.b);
                ResetSlidersRGB();
                OnValueChanged.Invoke(rgbColor);
                return;
            }

            hsv.s = square.x;
            hsv.v = square.y;
            ResetSlidersHSV();
            OnValueChanged.Invoke(Color.HSVToRGB(hsv.h, hsv.s, hsv.v));
        }

        public void SetHexCode(string hexCode)
        {
            if (type == (Type)255)
                return;
            if (!ColorParser.TryParse(hexCode, out Color c))
                return;
            if (type == Type.RGB)
            {
                rgb = (c.r, c.g, c.b);
                ResetSlidersRGB();
            }
            else if (type == Type.HSV)
            {
                Color.RGBToHSV(c, out hsv.h, out hsv.s, out hsv.v);
                ResetSlidersHSV();
            }
            OnValueChanged.Invoke(c);
        }

        public Color Color
        {
            get
            {
                if (type == Type.RGB)
                    return new Color(rgb.r, rgb.g, rgb.b, alpha);
                if (type == Type.HSV)
                {
                    Color c = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
                    c.a = alpha;
                    return c;
                }
                return new Color();
            }
            set
            {
                alpha = value.a;
                if (type == Type.RGB)
                {
                    rgb = (value.r, value.g, value.b);
                    ResetSlidersRGB();
                }
                else if (type == Type.HSV)
                {
                    Color.RGBToHSV(value, out hsv.h, out hsv.s, out hsv.v);
                    ResetSlidersHSV();
                }
            }
        }

        public Type ColorSpace
        {
            get => type;
            set
            {
                if (value == Type.RGB)
                    SwitchToRGB();
                else if (value == Type.HSV)
                    SwitchToHSV();
            }
        }
    }
}
