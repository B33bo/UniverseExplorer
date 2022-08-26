using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Btools.Components
{
    public class ColorPicker : MonoBehaviour
    {
        private Color _Color;

        [SerializeField]
        private Image ColorDisplay;

        [SerializeField]
        private TextMeshProUGUI ColorDisplayText;

        [SerializeField]
        private Image ColorSquareOutline;

        [SerializeField]
        private ColorSliderImages Alpha;

        [SerializeField]
        private ColorSliderImages Red, Green, Blue;

        [SerializeField]
        private Image Saturation, SaturationOff, ValueOn;

        [SerializeField]
        private TextMeshProUGUI HSVButtonText;
        private ColorType colorType;

        [SerializeField]
        private Slider A_slider;

        [SerializeField]
        private ColorTypeContainer RGB, HSV;

        [SerializeField]
        private Slider2D ColorSquare;

        [SerializeField]
        private TMP_InputField ColorCode;

        private bool IsEditingThisFrame = false;

        [SerializeField]
        private UnityEngine.Events.UnityEvent<Color> onColourChange;

        private enum ColorType : byte
        {
            RGBA,
            HSVA,
        }

        #region Methods for UnityEvents

        public void SetAlpha(float alpha)
        {
            if (IsEditingThisFrame)
                return;
            _Color.a = alpha / 255f;
            ResetColor();
            ResetAllExceptSquare();
            onColourChange.Invoke(_Color);
        }

        public void SetRed(float red)
        {
            if (IsEditingThisFrame)
                return;
            _Color.r = red / 255f;
            ResetColor();
            ResetHSV();
            onColourChange.Invoke(_Color);
        }

        public void SetGreen(float green)
        {
            if (IsEditingThisFrame)
                return;
            _Color.g = green / 255f;
            ResetColor();
            ResetHSV();
            onColourChange.Invoke(_Color);
        }

        public void SetBlue(float blue)
        {
            if (IsEditingThisFrame)
                return;
            _Color.b = blue / 255f;
            ResetColor();
            ResetHSV();
            onColourChange.Invoke(_Color);
        }

        public void SetHue(float hue)
        {
            if (IsEditingThisFrame)
                return;
            Color.RGBToHSV(_Color, out float H, out float S, out float V);
            _Color = Color.HSVToRGB(hue / 360f, S, V);
            ResetColor();
            ResetRGB();
            onColourChange.Invoke(_Color);
        }

        public void SetSaturation(float sat)
        {
            if (IsEditingThisFrame)
                return;
            Color.RGBToHSV(_Color, out float H, out float _, out float V);
            _Color = Color.HSVToRGB(H, sat / 255f, V);
            ResetColor();
            ResetRGB();
            onColourChange.Invoke(_Color);
        }

        public void SetValue(float value)
        {
            if (IsEditingThisFrame)
                return;
            Color.RGBToHSV(_Color, out float H, out float S, out float _);
            _Color = Color.HSVToRGB(H, S, value / 255f);
            ResetColor();
            ResetRGB();
            onColourChange.Invoke(_Color);
        }

        public void SetSatVal(Vector2 SatVal)
        {
            if (IsEditingThisFrame)
                return;
            Color.RGBToHSV(_Color, out float H, out float _, out float _);
            _Color = Color.HSVToRGB(H, SatVal.x / 255f, SatVal.y / 255f);
            ResetColor();
            ResetAllExceptSquare();
            onColourChange.Invoke(_Color);
        }

        public void SetHTMLString(string HTMLstring)
        {
            if (IsEditingThisFrame)
                return;
            IsEditingThisFrame = true;

            if (!ColorUtility.TryParseHtmlString(HTMLstring, out Color color))
            {
                Debug.Log("could not parse " + HTMLstring);
                IsEditingThisFrame = false;
                return;
            }
            _Color = color;

            Color.RGBToHSV(color, out float H, out float S, out float V);
            ColorSquare.Value = new Vector2(S * 255, V * 255);

            HSV.sliders[0].value = H * 360;
            HSV.sliders[1].value = S * 255;
            HSV.sliders[2].value = V * 255;

            A_slider.value = _Color.a * 255;

            RGB.sliders[0].value = _Color.r * 255;
            RGB.sliders[1].value = _Color.g * 255;
            RGB.sliders[2].value = _Color.b * 255;

            IsEditingThisFrame = false;
            ResetColor();
            onColourChange.Invoke(_Color);
        }
        #endregion

        private void ResetHSV()
        {
            if (IsEditingThisFrame)
                return;
            IsEditingThisFrame = true;

            Color.RGBToHSV(_Color, out float H, out float S, out float V);

            A_slider.value = _Color.a * 255;

            ColorSquare.Value = new Vector2(S * 255, V * 255);

            HSV.sliders[0].value = H * 360;
            HSV.sliders[1].value = S * 255;
            HSV.sliders[2].value = V * 255;

            ColorCode.text = "#" + ColorUtility.ToHtmlStringRGBA(_Color);

            IsEditingThisFrame = false;

            ResetColor();
        }

        private void ResetRGB()
        {
            IsEditingThisFrame = true;

            A_slider.value = _Color.a * 255;
            RGB.sliders[0].value = _Color.r * 255;
            RGB.sliders[1].value = _Color.g * 255;
            RGB.sliders[2].value = _Color.b * 255;

            Color.RGBToHSV(_Color, out float _, out float S, out float V);
            ColorSquare.Value = new Vector2(S * 255, V * 255);

            ColorCode.text = "#" + ColorUtility.ToHtmlStringRGBA(_Color);

            IsEditingThisFrame = false;
        }

        private void ResetAllExceptSquare()
        {
            IsEditingThisFrame = true;
            Color.RGBToHSV(_Color, out float H, out float S, out float V);

            A_slider.value = _Color.a * 255;

            RGB.sliders[0].value = _Color.r * 255;
            RGB.sliders[1].value = _Color.g * 255;
            RGB.sliders[2].value = _Color.b * 255;

            HSV.sliders[0].value = H * 360;
            HSV.sliders[1].value = S * 255;
            HSV.sliders[2].value = V * 255;

            ColorCode.text = "#" + ColorUtility.ToHtmlStringRGBA(_Color);

            IsEditingThisFrame = false;
        }

        public void HSVToggle()
        {
            colorType++;

            if (colorType > ColorType.HSVA)
                colorType = ColorType.RGBA;

            switch (colorType)
            {
                case ColorType.RGBA:
                    HSV.Target.SetActive(false);
                    RGB.Target.SetActive(true);
                    HSVButtonText.text = "RGB";
                    break;
                case ColorType.HSVA:
                    RGB.Target.SetActive(false);
                    HSV.Target.SetActive(true);
                    HSVButtonText.text = "HSV";
                    break;
                default:
                    RGB.Target.SetActive(false);
                    HSV.Target.SetActive(false);
                    HSVButtonText.text = colorType.ToString();
                    break;
            }
        }

        private void ResetColor()
        {
            Color.RGBToHSV(_Color, out float H, out float S, out float V);
            Color ColorWithFullAlpha = _Color;
            ColorWithFullAlpha.a = 1;

            ColorSquareOutline.color = Color.HSVToRGB(H, 1, 1);
            ColorDisplay.color = ColorWithFullAlpha;
            ColorDisplayText.color = Invert(ColorWithFullAlpha);

            Alpha.SetColor(_Color);
            Red.SetColor(_Color);
            Green.SetColor(_Color);
            Blue.SetColor(_Color);

            ValueOn.color = Color.HSVToRGB(H, S, 1);
            Saturation.color = Color.HSVToRGB(H, 1, V);
            SaturationOff.color = Color.HSVToRGB(H, 0, V);
        }

        private Color Invert(Color c) =>
            new Color
            {
                r = 1 - c.r,
                g = 1 - c.g,
                b = 1 - c.b,
                a = c.a,
            };

        [System.Serializable]
        public class ColorSliderImages
        {
            public Image on, off;
            public Color Mask;
            public bool Transparency;
            public bool HSV;

            public void SetColor(Color newColor)
            {
                Color offColor = newColor, onColor = newColor;

                if (Mask.r >= 1)
                {
                    offColor = new Color(0, offColor.g, offColor.b);
                    onColor = new Color(1, onColor.g, onColor.b);
                }

                if (Mask.g >= 1)
                {
                    offColor = new Color(offColor.r, 0, offColor.b);
                    onColor = new Color(onColor.r, 1, onColor.b);
                }

                if (Mask.b >= 1)
                {
                    offColor = new Color(offColor.r, offColor.g, 0);
                    onColor = new Color(onColor.r, onColor.g, 1);
                }

                if (Transparency)
                {
                    if (Mask.a >= 1)
                    {
                        offColor = new Color(offColor.r, offColor.g, 0);
                        onColor = new Color(onColor.r, onColor.g, 1);
                    }
                }
                else
                {
                    offColor.a = 1;
                    onColor.a = 1;
                }

                if (off)
                    off.color = offColor;
                if (on)
                    on.color = onColor;
            }
        }

        [System.Serializable]
        private class ColorTypeContainer
        {
            public GameObject Target;
            public Slider[] sliders;
        }
    }
}
