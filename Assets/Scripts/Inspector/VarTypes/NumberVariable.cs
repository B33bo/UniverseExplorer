using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Universe.Inspector
{
    public class NumberVariable : VariableUI
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private TMP_InputField inputField;
        private Type type;

        private enum Type
        {
            Float,
            Double,
            Integer,
        }

        protected override void InitValue(object value, object[] args)
        {
            if (value is float)
                type = Type.Float;
            else if (value is double)
                type = Type.Double;
            else if (value is int)
                type = Type.Integer;

            if (args.Length == 0)
            {
                slider.gameObject.SetActive(false);
                inputField.gameObject.SetActive(true);
                inputField.text = value.ToString();
            }
            else
            {
                slider.gameObject.SetActive(true);
                inputField.gameObject.SetActive(false);
                slider.minValue = ToFloat(args[0]);
                slider.maxValue = ToFloat(args[1]);
                slider.value = ToFloat(value);
            }
        }

        private float ToFloat(object o)
        {
            if (o is float f)
                return f;
            if (o is double d)
                return (float)d;
            if (o is int num)
                return (float)num;
            if (o is string s)
            {
                if (float.TryParse(s, out float f2))
                    return f2;
                return float.NaN;
            }
            throw new System.NotImplementedException();
        }

        private object ToObject(float val)
        {
            // don't use switch statement it doesn't work for some reason. casted to double
            if (type == Type.Float)
                return val;
            if (type == Type.Double)
                return (double)val;
            if (type == Type.Integer)
                return (int)val;
            throw new System.NotImplementedException();
        }

        public void SetFloatFromStr(string val)
        {
            if (float.TryParse(val, out float fVal))
                Set(fVal);
        }

        public void SetFloatFromSlider(float f)
        {
            Set(ToObject(f));
        }
    }
}
