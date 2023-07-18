using TMPro;
using UnityEngine;

namespace Universe.Inspector
{
    public class DropdownVariableUI : VariableUI
    {
        [SerializeField]
        private TMP_Dropdown dropdown;

        protected override void InitValue(object value, object[] args)
        {
            var names = value.GetType().GetEnumNames();

            for (int i = 0; i < names.Length; i++)
                dropdown.options.Add(new TMP_Dropdown.OptionData(names[i]));

            dropdown.value = (int)value;
        }

        public void SetDropdown(int val)
        {
            Set(val);
        }
    }
}
