using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btools.DevConsole
{
    public class DevConsoleVariable
    {
        private Func<object> getAction;
        private Action<string> setAction;
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsSetter = false;
        public readonly Type VarType;

        public object Get()
        {
            return getAction.Invoke();
        }

        public void Set(string value)
        {
            setAction.Invoke(value);
        }

        public DevConsoleVariable(string name, string description, Type type, Func<object> getAction)
        {
            this.getAction = getAction;
            this.setAction = null;
            Name = name.ToLower();
            Description = description;
            VarType = type;
        }

        public DevConsoleVariable(string name, string description, Type type, Func<object> getAction, Action<string> setAction)
        {
            this.getAction = getAction;
            this.setAction = setAction;
            Name = name.ToLower();
            Description = description;
            IsSetter = true;
            VarType = type;
        }

        public override string ToString()
        {
            string result = Name + " : " + VarType.ToString() + " (get";

            if (IsSetter)
                result += ", set";

            result += ")\n\n" + Description;
            return result;
        }
    }
}
