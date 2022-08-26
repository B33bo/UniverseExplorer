using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btools.DevConsole
{

    public class DevConsoleCommand
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string[] paramOptions;
        private Func<string[], string> Command;

        public DevConsoleCommand(string name, string description, Func<string[], string> command, params string[] paramOptions)
        {
            Name = name.ToLower();
            Description = description;
            Command = command;
            this.paramOptions = paramOptions;
        }

        public string Execute(string[] args)
        {
            return Command.Invoke(args);
        }

        public override string ToString()
        {
            string result = Name;
            for (int i = 0; i < paramOptions.Length; i++)
                result += "," + paramOptions[i];
            result += "\n" + Description;
            return result;
        }
    }
}
