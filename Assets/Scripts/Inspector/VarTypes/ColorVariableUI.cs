using Btools.Components;
using UnityEngine;
using Universe.Inspector;

namespace Universe
{
    public class ColorVariableUI : VariableUI
    {
        [SerializeField]
        private ColorPicker color;
        private bool hsv;

        protected override void InitValue(object value, object[] args)
        {
            if (value is ColorHSV hsv)
            {
                this.hsv = true;
                color.Color = hsv;
            }
            else
                color.Color = (Color)value;
        }

        public void SetC(Color col)
        {
            if (!initialised)
                return;

            if (hsv)
                Set((ColorHSV)col);
            else
                Set(col);
        }
    }
}
