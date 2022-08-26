using System;
using System.Reflection;
using System.Collections.Generic;
using Btools.Extensions;

namespace Btools.DevConsole
{
    public static class DevCommands
    {
        internal static Dictionary<string, DevConsoleCommand> Commands = BuiltInDevCommands.Builtins;
        internal static Dictionary<string, DevConsoleVariable> Variables = BuiltInDevVariables.Builtins;
        internal static Dictionary<string, string> pureVariables = new Dictionary<string, string>();

        private static List<string> _history = new List<string>();

        //ensures there is no way to maniputlate the history
        public static string[] History => _history.ToArray();

        public static void Register(DevConsoleCommand Command)
        {
            if (Commands.ContainsKey(Command.Name))
            {
                Commands[Command.Name] = Command;
                return;
            }
            Commands.Add(Command.Name, Command);
        }

        public static void Register(string CommandName, string Description, Func<string[], string> CommandAction, params string[] paramOptions)
        {
            CommandName = CommandName.ToLower();
            DevConsoleCommand command = new DevConsoleCommand(CommandName, Description, CommandAction, paramOptions);
            if (Commands.ContainsKey(CommandName))
            {
                Commands[CommandName] = command;
                return;
            }
            Commands.Add(CommandName, command);
        }

        public static void RegisterVar(DevConsoleVariable variable)
        {
            if (Variables.ContainsKey(variable.Name))
            {
                Variables[variable.Name] = variable;
                return;
            }
            Variables.Add(variable.Name, variable);
        }

        public static void RegisterVar(string name, string data)
        {
            pureVariables.Add(name, data);
            Variables.Add(name, new DevConsoleVariable(name, "User-defined variable", typeof(string), () => pureVariables[name], x => pureVariables[name] = x));
        }

        public static string AutoComplete(string unfinishedString)
        {
            char lastOccurrenceOfImportantChar = LastOccurrenceOfImportantCharacter(unfinishedString, out int i);

            if (lastOccurrenceOfImportantChar == '$')
                // A variable needs to be auto completed
                return AutoCompleteVariable(unfinishedString, i);
            else if (lastOccurrenceOfImportantChar == ';')
                //a command needs to be auto completed
                return AutoCompleteCommand(unfinishedString, i);
            else if (lastOccurrenceOfImportantChar == ',')
                //a parameter needs to be auto completed
                return AutoCompleteParameter(unfinishedString, i);

            //unknown autocompletion type. normally this would never happen
            return unfinishedString;
        }

        private static string AutoCompleteParameter(string unfinishedString, int index)
        {
            //Example: testCMD1,hello,abc;testCMD2,hello,hi
            string lastCommand = "";
            for (int i = unfinishedString.Length - 1; i >= 0; i--)
            {
                if (unfinishedString[i] == ';')
                    break;
                lastCommand = unfinishedString[i] + lastCommand;
            }

            //the bit before the last command {testCMD1,hello,abc;}
            string beforeLastCommand = unfinishedString.Substring(0, unfinishedString.Length - lastCommand.Length);

            //[testCMD2, hello, hi]
            var commandParams = lastCommand.SplitEscaped(',');

            if (!Commands.ContainsKey(commandParams[0]))
                //{testCMD2 is not a command}
                return unfinishedString;

            //testCMD2
            var command = Commands[commandParams[0]];

            //so if you put too many parameters, it doesn't crash
            //like if I did {testCMD2,a,b,c,d,e} and it only supported {testCMD2,a,b,c}
            if (command.paramOptions.Length <= commandParams.Length - 2)
                return unfinishedString;

            //the parameter autocomplete choices
            string[] choices = command.paramOptions[commandParams.Length - 2].Split('|');

            //{hi}
            string unfinishedChoice = unfinishedString.Substring(index + 1);

            //{hippo} if you complete hi to hippo
            string finishedChoice = "";

            for (int i = 0; i < choices.Length; i++)
            {
                if (!choices[i].StartsWith(unfinishedChoice))
                    continue;
                finishedChoice = choices[i];
                break;
            }

            string commandAsStringWithoutLastParam = "";
            for (int i = 0; i < commandParams.Length - 1; i++)
            {
                commandAsStringWithoutLastParam += commandParams[i] + ",";
            }

            //beforeLastCommand = testCMD1,hello,abc;
            //commandAsStringWithoutLastParam = testCMD2,hello,
            //finishedChoice = hi{ppo}

            return beforeLastCommand + commandAsStringWithoutLastParam + finishedChoice;
        }

        private static string AutoCompleteCommand(string unfinishedString, int index)
        {
            string previousCommands = unfinishedString.Substring(0, index);
            string lastCommand;
            if (index == 0)
                lastCommand = unfinishedString.Substring(index);
            else
                lastCommand = unfinishedString.Substring(index + 1);

            if (previousCommands == "")
                return AutoCompleteLastCommand(lastCommand);

            return previousCommands + ";" + AutoCompleteLastCommand(lastCommand);
        }

        private static string AutoCompleteVariable(string unfinishedString, int index)
        {
            string commandBeforeVar = unfinishedString.Substring(0, index);
            string commandAfterVar = unfinishedString.Substring(index + 1);

            foreach (var variable in Variables)
            {
                if (variable.Key.StartsWith(commandAfterVar))
                    return commandBeforeVar + "$" + variable.Key;
            }
            return unfinishedString;
        }

        private static char LastOccurrenceOfImportantCharacter(string unfinishedString, out int i)
        {
            for (i = unfinishedString.Length - 1; i >= 0; i--)
            {
                if (unfinishedString[i] == '$')
                    return '$';
                if (unfinishedString[i] == ';')
                    return ';';
                if (unfinishedString[i] == ',')
                    return ',';
            }

            i = 0;
            return ';';
        }

        private static string AutoCompleteLastCommand(string lastCommand)
        {
            foreach (var command in Commands)
            {
                if (command.Key.StartsWith(lastCommand))
                    return lastCommand + command.Key.Substring(lastCommand.Length); //keep capitilisation
            }
            return "";
        }

        private static string ExecuteSingleCommand(string[] Parameters)
        {
            string CommandName = Parameters[0];

            if (CommandName.StartsWith("$"))
                return VariableCommand(CommandName);

            CommandName = CommandName.ToLower();
            if (!Commands.ContainsKey(CommandName))
                return $"<color=#FF0000>Command '{CommandName}' Not found.</color>";

            for (int i = 0; i < Parameters.Length; i++)
            {
                if (Parameters[i].StartsWith("$") && Parameters[i].Length > 1)
                {
                    string varName = Parameters[i].Substring(1);
                    if (!Variables.ContainsKey(varName))
                        return $"<color=#FF0000>{varName} is not a variable</color>";
                    Parameters[i] = Variables[varName].Get().ToString();
                }
            }

            return Commands[CommandName].Execute(Parameters);
        }

        private static string VariableCommand(string Command)
        {
            Command = Command.Replace(" ", "");

            if (!Command.Contains("="))
                return Variables[Command.Substring(1)].Get().ToString();

            string[] varNameAndNewValue = Command.Split('=');
            varNameAndNewValue[0] = varNameAndNewValue[0].Substring(1);

            if (varNameAndNewValue[1].Length > 0 && varNameAndNewValue[1][0] == '$' && Variables.ContainsKey(varNameAndNewValue[1].Substring(1)))
                //$var1=$var2
                varNameAndNewValue[1] = Variables[varNameAndNewValue[1].Substring(1)].Get().ToString();

            if (!Variables.ContainsKey(varNameAndNewValue[0]))
                RegisterVar(varNameAndNewValue[0], varNameAndNewValue[1]);

            Variables[varNameAndNewValue[0]].Set(varNameAndNewValue[1]);
            return Variables[varNameAndNewValue[0]].Get().ToString();
        }

        public static string Execute(string Command)
        {
            _history.Add(Command);
            string[] commands = Command.SplitEscaped(';');
            string result = "";

            for (int i = 0; i < commands.Length; i++)
            {
                var commandParams = commands[i].SplitEscaped(',');
                result += ExecuteSingleCommand(commandParams) + "\n";
            }

            return result;
        }
    }
}
