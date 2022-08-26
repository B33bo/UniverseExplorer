using UnityEngine;
using TMPro;
using Btools.utils;

namespace Btools.DevConsole
{
    public class DevConsoleWindow : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField commandInput;

        [SerializeField]
        private TextMeshProUGUI PlaceholderText;

        [SerializeField]
        private TextMeshProUGUI CommandOutput;

        [SerializeField]
        private string CommandOnStart;

        private void Start()
        {
            Application.logMessageReceived += LogMessageRevieved;

#if UNITY_EDITOR
            if (!CommandOnStart.StartsWith("//"))
                Debug.Log("DV:\n" + DevCommands.Execute(CommandOnStart));
#endif
        }

        private void LogMessageRevieved(string condition, string stackTrace, LogType type)
        {
            string ColorString = type switch
            {
                LogType.Assert => "<#FF0000>",
                LogType.Error => "<#FF0000>",
                LogType.Exception => "<#FF0000>",
                LogType.Log => "<#CCCCCC>",
                LogType.Warning => "<#FFFF00>",
                _ => "<#FFFFFF>",
            };

            CommandOutput.text += $"Log> {ColorString}{condition}</color>\n";
        }

        public void TypedWord(string text)
        {
            string newString = DevCommands.AutoComplete(text);
            PlaceholderText.text = newString;
        }

        public void Run()
        {
            Run(commandInput.text);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Tab))
                return;
            if (!commandInput.isFocused)
                return;

            commandInput.text = DevCommands.AutoComplete(commandInput.text);
            commandInput.caretPosition = commandInput.text.Length;
        }

        public void Run(string text)
        {
            CommandOutput.text += $"<#00FA05>> {text}</color>\n";
            string commandCallback = DevCommands.Execute(text);
            CommandOutput.text += commandCallback + "\n";
            CommandOutput.rectTransform.anchoredPosition = new Vector2(0, CommandOutput.renderedHeight + 50);

            if (commandCallback.Contains("$clearConsole"))
                CommandOutput.text = "";
            else if (commandCallback.Contains("$resetConsole"))
            {
                GetComponent<Components.Window>().IsFullscreen = false;
                transform.position = Vector2.zero;
            }
        }
    }
}
