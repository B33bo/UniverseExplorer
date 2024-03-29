using TMPro;
using UnityEngine;

namespace Universe.Inspector
{
    public class StringVariableUI : VariableUI
    {
        [SerializeField]
        private TMP_InputField inputField;

        protected override void InitValue(object value, object[] args)
        {
            if (value != null)
                inputField.text = value.ToString();
        }
    }
}
