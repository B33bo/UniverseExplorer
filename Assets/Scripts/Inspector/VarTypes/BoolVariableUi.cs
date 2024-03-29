using UnityEngine;
using UnityEngine.UI;

namespace Universe.Inspector
{
    public class BoolVariableUi : VariableUI
    {
        [SerializeField]
        protected Toggle toggle;
        protected override void InitValue(object value, object[] args)
        {
            toggle.isOn = (bool)value;
        }

        public void SetBool(bool value)
        {
            Set(value);
        }
    }
}
