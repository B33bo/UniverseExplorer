using TMPro;
using UnityEngine;

namespace Universe.Inspector
{
    public abstract class VariableUI : MonoBehaviour
    {
        public float Height;
        protected bool initialised = false;

        [SerializeField]
        private TextMeshProUGUI label;
        protected Variable variable;

        public void Init(Variable variable, object[] args)
        {
            this.variable = variable;
            label.text = variable.VariableName;
            InitValue(variable.GetValue(), args);
            initialised = true;
        }

        protected abstract void InitValue(object value, object[] args);

        public void Set(object value)
        {
            if (!initialised)
                return;
            variable.SetValue(value);
        }
    }
}
