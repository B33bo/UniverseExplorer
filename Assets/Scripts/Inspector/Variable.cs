using System;

namespace Universe.Inspector
{
    public class Variable
    {
        public string VariableName { get; set; }
        public Func<object> GetValue { get; set; }
        public Action<object> SetValue { get; set; }

        public Variable(string variableName, Func<object> getValue, Action<object> setValue)
        {
            VariableName = variableName;
            GetValue = getValue;
            SetValue = setValue;
        }
    }
}
