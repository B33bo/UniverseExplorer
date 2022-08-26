using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using System.Collections.Generic;
using Btools.Extensions;
using Btools.utils;

namespace Btools.DevConsole
{
    internal static class BuiltInDevCommands
    {
        public static Dictionary<string, DevConsoleCommand> Builtins => new Dictionary<string, DevConsoleCommand>()
        {
            {"", new DevConsoleCommand("", "", x => "") },
            {"no_op", new DevConsoleCommand("no_op", "no operation; do nothing", x => "") },
            {"quit", new DevConsoleCommand("quit", "quits the game", Quit, "quit|crash") },
            {"exit", new DevConsoleCommand("exit", "same as quit", Quit, "quit|crash") },
            {"log", new DevConsoleCommand("log", "logs to the unity console", LogMessage, "log|warn|error|assert|except|all") },
            {"echo", new DevConsoleCommand("echo", "returns the parameter to the DevConsole (not the unity console)", x => x[1], "message") },
            {"scene", new DevConsoleCommand("scene", "load a scene in unity", LoadScene, "sceneName", "single|add|unload")},
            {"clear", new DevConsoleCommand("clear", "clears the devConsole by sending a clear command ($ + clearConsole)", x => "$clearConsole") },
            {"objects", new DevConsoleCommand("objects", "list all the objects in all scenes", ListObjects, "tree|flat") },
            {"destroy", new DevConsoleCommand("destroy", "destroy a gameobject at the specific path", Destroy, "sceneName/objectParent/object") },
            {"posofobj", new DevConsoleCommand("posofobj", "get/set the position of an object", Position, "objpath", "x", "y", "z") },
            {"rotofobj", new DevConsoleCommand("rotofobj", "get/set the rotation of an object", Rotation, "objpath", "rot", "y", "z", "w")},
            {"scaleofobj", new DevConsoleCommand("scaleofobj", "get/set the scale of an object", Scale, "objpath", "x", "y", "z")},
            {"clone", new DevConsoleCommand("clone", "makes a copy of the object", Clone, "objpath", "name")},
            {"parent", new DevConsoleCommand("parent", "sets the parent of an object to another object", SetParent, "child", "parent")},
            {"reset", new DevConsoleCommand("reset", "sends a command to the console to reset it ($ + resetConsole)", x => "$resetConsole") },
            {"varedit", new DevConsoleCommand("varedit", "change the variable of a component", ComponentVariable, "object", "component name", "var name", "new value") },
            {"components", new DevConsoleCommand("components", "list the components of an object", ComponentsOf, "object") },
            {"help", new DevConsoleCommand("help", "gives a description of the variable/command", Help, "comand name|variable name", "command|variable")},
            {"camsize", new DevConsoleCommand("camsize", "set the cam size of any camera", CamSize, "size", "object") },
            {"playerprefs", new DevConsoleCommand("playerprefs", "gets/sets the value of a player pref", PlayerPrefsEdit, "int|string|float|delete", "name", "newValue") },
            {"history", new DevConsoleCommand("history", "read the command history", History) },
            {"splash", new DevConsoleCommand("splash", "show the splash screen", Splash) },
            {"bind", new DevConsoleCommand("bind", "bind a command to a key", Bind, "key", "Held|Down|Up", "Command") },
            {"unbind", new DevConsoleCommand("unbind", "unbind a dev key binds", Unbind, "key") },
            {"update", new DevConsoleCommand("update", "make a command run every frame", Update, "Update|Fixed", "Command") },
            {"unregister_update", new DevConsoleCommand("unregister_update", "remove all update commands", UnregisterUpdates) },
            {"if", new DevConsoleCommand("if", "run a command if the condition is met", If, "condition", "command") },
            {"delay", new DevConsoleCommand("delay", "run a command after the delay", Delay, "length", "seconds|real_time|frames", "Command") },
            {"set", new DevConsoleCommand("set", "set a variable", Set, "varName", "varData") },
            {"get", new DevConsoleCommand("get", "get a variable", Get, "varName") },
            {"run", new DevConsoleCommand("run", "run a variable", Run, "Command") },
            {"new_command", new DevConsoleCommand("new_command", "make a new command that runs other commands", NewCommand, "Name", "Description", "Command", "Params") },
            {"remove_command", new DevConsoleCommand("remove_command", "remove a command", RemoveCommand, "CommandName") },
            {"remove_variable", new DevConsoleCommand("remove_variable", "remove a variable", RemoveVariable, "VarName") },
            {"fall", new DevConsoleCommand("fall", "make all objects fall", Fall, "2D|3D") }
        };

        private static string Quit(string[] Parameters)
        {
            if (Parameters.Length <= 1 || Parameters[1].ToLower() != "crash")
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return "bye!";
            }

            UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);

            return "Force crash";
        }

        private static string LogMessage(string[] Parameters)
        {
            if (Parameters.Length < 3)
            {
                Debug.Log(Parameters[1]);
                return Parameters[1];
            }

            LogType logType = (LogType)(-1);
            switch (Parameters[2].ToLower())
            {
                default:
                    Debug.LogWarning($"{Parameters[2]} is not a log type");
                    Debug.Log(Parameters[1]);
                    break;
                case "log":
                    Debug.Log(Parameters[1]);
                    logType = LogType.Log;
                    break;
                case "warn":
                case "warning":
                    Debug.LogWarning(Parameters[1]);
                    logType = LogType.Warning;
                    break;
                case "error":
                    Debug.LogError(Parameters[1]);
                    logType = LogType.Error;
                    break;
                case "assert":
                case "assertion":
                    Debug.LogAssertion(Parameters[1]);
                    logType = LogType.Assert;
                    break;
                case "exception":
                case "except":
                    Debug.LogException(new Exception(Parameters[1]));
                    logType = LogType.Exception;
                    break;
                case "all":
                    Debug.Log(Parameters[1]);
                    Debug.LogWarning(Parameters[1]);
                    Debug.LogError(Parameters[1]);
                    Debug.LogAssertion(Parameters[1]);
                    Debug.LogException(new Exception(Parameters[1]));
                    break;
            }
            string ColorString = logType switch
            {
                LogType.Assert => "<color=#FF0000>",
                LogType.Error => "<color=#FF0000>",
                LogType.Exception => "<color=#FF0000>",
                LogType.Log => "<color=#CCCCCC>",
                LogType.Warning => "<color=#FFFF00>",
                _ => "<color=#FFFFFF>",
            };

            return ColorString + Parameters[1] + "</color>";
        }

        private static string LoadScene(string[] Parameters)
        {
            if (Parameters.Length == 1)
            {
                var scenes = SceneManager.sceneCount;
                string s = "";
                for (int i = 0; i < scenes; i++)
                {
                    var currentScene = SceneManager.GetSceneAt(i);
                    if (currentScene == SceneManager.GetActiveScene())
                        s += "<color=#00FF00>" + currentScene.name + "</color>";
                    else
                        s += currentScene.name + "\n";
                }
                return s;
            }

            if (Parameters.Length == 2)
            {
                SceneManager.LoadScene(Parameters[1]);
                return Parameters[1];
            }

            if (Parameters.Length >= 3)
            {
                switch (Parameters[2].ToLower())
                {
                    default:
                        return $"{Parameters[2]} is invalid";
                    case "unload":
                    case "subtract":
                    case "-":
                        SceneManager.UnloadSceneAsync(Parameters[1]);
                        return "Unloaded " + Parameters[1];
                    case "load":
                    case "single":
                    case "=":
                        SceneManager.LoadScene(Parameters[1]);
                        return "loaded " + Parameters[1];
                    case "add":
                    case "+":
                        SceneManager.LoadScene(Parameters[1], LoadSceneMode.Additive);
                        return "added " + Parameters[1];
                }
            }

            return "error";
        }

        private static string ListObjects(string[] Parameters)
        {
            bool tree = true;
            for (int i = 1; i < Parameters.Length; i++)
            {
                if (Parameters[i].Replace(" ", "") == "flat")
                    tree = false;
            }

            if (!tree)
            {
                var Gameobjects = GameObject.FindObjectsOfType<GameObject>();
                string s = "";
                for (int i = 0; i < Gameobjects.Length; i++)
                    s += Gameobjects[i].name + "\n";
                return s;
            }

            string output = "";
            for (int i = -1; i < SceneManager.sceneCount; i++)
            {
                Scene currentScene;
                if (i == -1)
                    currentScene = GameObjectUtils.DontDestroyOnLoadScene;
                else
                    currentScene = SceneManager.GetSceneAt(i);

                output += $"<color=#FF0000>{currentScene.name}</color>\n";
                var objectsInScene = currentScene.GetRootGameObjects();
                foreach (var gameObj in objectsInScene)
                {
                    output += GetObjectsInsideOf(gameObj.transform, 1) + "\n";
                }
            }
            return output;
        }

        private static string GetObjectsInsideOf(Transform TargetObject, int tabIndex)
        {
            string[] colourCodes = new string[]
            {
                "<color=#FF0000>",
                "<color=#FFFF00>",
                "<color=#00FF00>",
                "<color=#00FFFF>",
                "<color=#0000FF>",
                "<color=#FF00FF>",
                "<color=#0000FF>",
                "<color=#00FFFF>",
                "<color=#00FF00>",
                "<color=#FFFF00>",
            };
            string pipes = "";

            //load all of the pipes ('|')
            for (int i = 0; i < tabIndex; i++)
                pipes += $"{colourCodes[i % colourCodes.Length]}\u2502</color>";

            string colourCode = colourCodes[tabIndex % colourCodes.Length];
            string FinalString = pipes + colourCode + TargetObject.name + "</color>";
            Transform[] children = TargetObject.transform.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].transform.parent != TargetObject.transform)
                    continue;

                FinalString += "\n" + GetObjectsInsideOf(children[i], tabIndex + 1);
            }

            return FinalString;
        }

        private static string Destroy(string[] Parameters)
        {
            List<string> failedObjects = new List<string>();

            for (int i = 1; i < Parameters.Length; i++)
            {
                try
                {
                    var currentGm = GameObjectUtils.FindPath(Parameters[i].SplitEscaped('/'));
                    MonoBehaviour.Destroy(currentGm);
                }
                catch (Exception exc)
                {
                    failedObjects.Add(Parameters[i] + ", " + exc.Message);
                }
            }

            string failedObjectsString = null;
            for (int i = 0; i < failedObjects.Count; i++)
                failedObjectsString += $"Failed To Destroy: {failedObjects[i]}\n";

            return failedObjectsString ?? "Success";
        }

        private static string Position(string[] Parameters)
        {
            GameObject gameObject = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));
            if (Parameters.Length == 4)
                gameObject.transform.position = new Vector2(float.Parse(Parameters[2]), float.Parse(Parameters[3]));
            else if (Parameters.Length > 4)
                gameObject.transform.position = new Vector3(float.Parse(Parameters[2]), float.Parse(Parameters[3]), float.Parse(Parameters[4]));

            return gameObject.transform.position.ToString();
        }

        private static string Rotation(string[] Parameters)
        {
            GameObject gameObject = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));

            if (Parameters.Length == 3)
                gameObject.transform.rotation = Quaternion.Euler(0, 0, float.Parse(Parameters[2]));
            else if (Parameters.Length == 5)
                gameObject.transform.rotation = Quaternion.Euler(float.Parse(Parameters[2]), float.Parse(Parameters[3]), float.Parse(Parameters[4]));
            else if (Parameters.Length > 5)
                gameObject.transform.rotation = new Quaternion(float.Parse(Parameters[2]), float.Parse(Parameters[3]), float.Parse(Parameters[4]), float.Parse(Parameters[5]));

            return gameObject.transform.rotation.eulerAngles.ToString();
        }

        private static string Scale(string[] Parameters)
        {
            GameObject gameObject = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));

            if (Parameters.Length == 3)
                gameObject.transform.localScale *= float.Parse(Parameters[2]);
            else if (Parameters.Length == 4)
                gameObject.transform.localScale = new Vector3(float.Parse(Parameters[2]), float.Parse(Parameters[3]));
            else if (Parameters.Length > 4)
                gameObject.transform.localScale = new Vector3(float.Parse(Parameters[2]), float.Parse(Parameters[3]), float.Parse(Parameters[4]));

            return gameObject.transform.localScale.ToString();
        }

        private static string Clone(string[] Parameters)
        {
            GameObject gameObject = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));
            GameObject newGM;

            if (Parameters.Length > 2)
            {
                newGM = MonoBehaviour.Instantiate(gameObject);
                newGM.name = Parameters[2];
            }
            else
                newGM = MonoBehaviour.Instantiate(gameObject);

            return "Created " + newGM.name;
        }

        private static string SetParent(string[] Parameters)
        {
            GameObject target = GameObjectUtils.FindPath(Parameters[1]);
            if (Parameters[2] == "null")
            {
                target.transform.parent = null;
                return $"{target.name} now has no parent. you monster.";
            }
            GameObject parent = GameObjectUtils.FindPath(Parameters[2]);

            target.transform.parent = parent.transform;

            return $"{target.name} is now parent of {parent.name}";
        }

        //"varedit",GameObject,ComponentName,VarName,NewVal?
        private static string ComponentVariable(string[] Parameters)
        {
            GameObject target = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));
            Component component = target.GetComponent(Parameters[2]);
            Type type = component.GetType();

            var Target = type.GetMember(Parameters[3])[0];

            MemberType memberType = MemberType.Unknown;

            PropertyInfo propertyInfo = null;
            FieldInfo fieldInfo = null;
            MethodInfo methodInfo = null;

            propertyInfo = Target as PropertyInfo;
            memberType = MemberType.Property;
            if (propertyInfo is null)
            {
                fieldInfo = Target as FieldInfo;
                memberType = MemberType.Field;

                if (fieldInfo is null)
                {
                    memberType = MemberType.Method;
                    methodInfo = Target as MethodInfo;

                    if (methodInfo is null)
                        memberType = MemberType.Unknown;
                }
            }

            if (Parameters.Length < 4)
            {
                return memberType switch
                {
                    MemberType.Field => fieldInfo.GetValue(component).ToString(),
                    MemberType.Property => propertyInfo.GetValue(component).ToString(),
                    MemberType.Method => methodInfo.Invoke(component, new object[0]).ToString(),
                    _ => "unknown member type " + Target.GetType(),
                };
            }

            switch (memberType)
            {
                case MemberType.Field:
                    fieldInfo.SetValue(component, Convert.ChangeType(Parameters[4], fieldInfo.FieldType));
                    break;
                case MemberType.Property:
                    propertyInfo.SetValue(component, Convert.ChangeType(Parameters[4], propertyInfo.PropertyType));
                    break;
                case MemberType.Method:
                    methodInfo.Invoke(component, new object[0]);
                    break;
                default:
                    return "unknown member type " + Target.GetType();
            }

            return Parameters[4];
        }

        private static string ComponentsOf(string[] Parameters)
        {
            GameObject gameObject = GameObjectUtils.FindPath(Parameters[1].SplitEscaped('/'));

            Component[] components = gameObject.GetComponents(typeof(Component));
            string s = "";

            for (int i = 0; i < components.Length; i++)
            {
                s += components[i].GetType().Name + "\n";
            }
            return s;
        }

        private static string Help(string[] Parameters)
        {
            if (Parameters.Length == 1)
            {
                string s = "COMMANDS:\n";
                foreach (var command in DevCommands.Commands)
                {
                    s += command.Key + "\n";
                }
                s += "\nVARIABLES:\n";
                foreach (var command in DevCommands.Variables)
                {
                    s += command.Key + "\n";
                }
                return s;
            }
            if (Parameters.Length == 2)
            {
                if (DevCommands.Commands.ContainsKey(Parameters[1]))
                    return DevCommands.Commands[Parameters[1]].ToString();

                if (!DevCommands.Variables.ContainsKey(Parameters[1]))
                    return $"variable or command {Parameters[1]} does not exist.";

                return DevCommands.Variables[Parameters[1]].ToString();
            }

            if (Parameters[2] == "variable")
            {
                if (!DevCommands.Variables.ContainsKey(Parameters[1]))
                    return $"variable {Parameters[1]} does not exist.";

                return DevCommands.Variables[Parameters[1]].ToString();
            }

            if (DevCommands.Commands.ContainsKey(Parameters[1]))
                return DevCommands.Commands[Parameters[1]].ToString();

            return $"command {Parameters[1]} does not exist.";
        }

        private static string CamSize(string[] Parameters)
        {
            if (Parameters.Length == 1)
                return Camera.main.orthographicSize.ToString();

            Camera cameraChosen;
            float size = float.Parse(Parameters[1]);
            if (Parameters.Length == 2)
                cameraChosen = Camera.main;
            else
                cameraChosen = GameObjectUtils.FindPath(Parameters[2].SplitEscaped('/')).GetComponent<Camera>();

            cameraChosen.fieldOfView = size;
            cameraChosen.orthographicSize = size;
            return cameraChosen.orthographicSize.ToString();
        }

        private static string PlayerPrefsEdit(string[] Parameters)
        {
            switch (Parameters[1])
            {
                default:
                    return $"<color=#FF0000>Unknown Type '{Parameters[1]}' must be int, float, string, or delete to delete a key</color>";
                case "int":
                    if (Parameters.Length > 3)
                        PlayerPrefs.SetInt(Parameters[2], int.Parse(Parameters[3]));
                    return PlayerPrefs.GetInt(Parameters[2]).ToString();
                case "float":
                    if (Parameters.Length > 3)
                        PlayerPrefs.SetFloat(Parameters[2], float.Parse(Parameters[3]));
                    return PlayerPrefs.GetFloat(Parameters[2]).ToString();
                case "string":
                    if (Parameters.Length > 3)
                        PlayerPrefs.SetString(Parameters[2], Parameters[3]);
                    return PlayerPrefs.GetString(Parameters[2]);
                case "delete":
                    PlayerPrefs.DeleteKey(Parameters[2]);
                    return (!PlayerPrefs.HasKey(Parameters[2])).ToString();
            }
        }

        private static string History(string[] Parameters)
        {
            string s = "command history:\n";

            var history = DevCommands.History;
            for (int i = 0; i < history.Length; i++)
            {
                s += history[i] + "\n";
            }
            s += "END OF HISTORY";
            return s;
        }

        private static string Splash(string[] Parameters)
        {
            UnityEngine.Rendering.SplashScreen.Begin();
            return "";
        }

        private static string Bind(string[] Parameters)
        {
            DevKeyBind devKeyBind = new GameObject($"bind for {Parameters[1]}").AddComponent<DevKeyBind>();
            devKeyBind.key = (KeyCode)Enum.Parse(typeof(KeyCode), Parameters[1]);
            devKeyBind.type = Parameters[2].ToLower() switch
            {
                "held" => DevKeyBind.Type.Held,
                "heldfixedupdate" => DevKeyBind.Type.HeldFixedUpdate,
                "down" => DevKeyBind.Type.Down,
                "up" => DevKeyBind.Type.Up,
                _ => DevKeyBind.Type.Held,
            };
            devKeyBind.Command = Parameters[3];

            return devKeyBind.name;
        }

        private static string Unbind(string[] Parameters)
        {
            KeyCode keyToRemove = (KeyCode)Enum.Parse(typeof(KeyCode), Parameters[1]);

            for (int i = 0; i < DevKeyBind.Instances.Count; i++)
            {
                if (DevKeyBind.Instances[i].Item1 == keyToRemove)
                {
                    DevKeyBind.Instances[i].Item2.Unbind();
                    DevKeyBind.Instances.RemoveAt(i);
                    i--;
                }
            }

            return "unbinded all instances of " + keyToRemove.ToString();
        }

        private static string Update(string[] Parameters)
        {
            var update = new GameObject($"Registration to {Parameters[1]}").AddComponent<DevUpdateOrFixed>();
            update.type = Parameters[1].ToLower() switch
            {
                "awake" => DevUpdateOrFixed.Type.Awake,
                "start" => DevUpdateOrFixed.Type.Start,
                "update" => DevUpdateOrFixed.Type.Update,
                "fixed" => DevUpdateOrFixed.Type.FixedUpdate,
                _ => DevUpdateOrFixed.Type.Awake,
            };
            update.Command = Parameters[2];

            if (update.type == DevUpdateOrFixed.Type.Awake)
                Debug.Log(DevCommands.Execute(update.Command));
            return update.name;
        }

        private static string UnregisterUpdates(string[] Parameters)
        {
            while (DevUpdateOrFixed.Instances.Count > 0)
            {
                DevUpdateOrFixed.Instances[0].Unbind();
                DevUpdateOrFixed.Instances.RemoveAt(0);
            }
            return "cleared";
        }

        private static string If(string[] Parameters)
        {
            if (Parameters[1].ToLower() == "true")
                return DevCommands.Execute(Parameters[2]);
            else if (Parameters.Length > 3)
                return DevCommands.Execute(Parameters[3]);
            return "";
        }

        private static string Delay(string[] Parameters)
        {
            utils.EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(DelayEnumerator(Parameters));
            return "";
        }

        private static System.Collections.IEnumerator DelayEnumerator(string[] Parameters)
        {
            switch (Parameters[2].ToLower())
            {
                case "seconds":
                default:
                    yield return new WaitForSeconds(float.Parse(Parameters[1]));
                    break;
                case "real_time":
                    yield return new WaitForSecondsRealtime(float.Parse(Parameters[1]));
                    break;
                case "frames":
                    yield return new WaitForFrames(int.Parse(Parameters[1]));
                    break;
            }

            Debug.Log(DevCommands.Execute(Parameters[3]));
            yield break;
        }

        private static string Set(string[] Parameters)
        {
            DevCommands.Variables[Parameters[1]].Set(Parameters[2]);
            return DevCommands.Variables[Parameters[1]].Get().ToString();
        }

        private static string Get(string[] Parameters)
        {
            return DevCommands.Variables[Parameters[1]].Get().ToString();
        }

        private static string Run(string[] Parameters)
        {
            string callback = "";
            for (int i = 1; i < Parameters.Length; i++)
            {
                callback += "\n" + DevCommands.Execute(Parameters[1]);
            }
            return callback.Substring(1);
        }

        private static string NewCommand(string[] Parameters)
        {
            string[] commandParams = new string[Parameters.Length - 4];

            for (int i = 4; i < Parameters.Length; i++)
                commandParams[i - 4] = Parameters[i];

            DevCommands.Register(Parameters[1], Parameters[2], x => DevCommands.Execute(Parameters[3]), commandParams);
            return DevCommands.Commands[Parameters[1].ToLower()].ToString();
        }

        private static string RemoveCommand(string[] Parameters)
        {
            DevCommands.Commands.Remove(Parameters[1]);

            if (Parameters[1] == "!ALL!")
                DevCommands.Commands.Clear();

            return "Removed command " + Parameters[1];
        }

        private static string RemoveVariable(string[] Parameters)
        {
            DevCommands.Variables.Remove(Parameters[1]);

            if (Parameters[1] == "!ALL!")
                DevCommands.Variables.Clear();

            return "Removed variable " + Parameters[1];
        }

        private static string Fall(string[] Parameters)
        {
            Transform[] allGameObjects = GameObject.FindObjectsOfType<Transform>();
            bool TwoDimensions = true;
            if (Parameters.Length > 1)
                TwoDimensions = Parameters[1] != "3D";

            float lowestYlevel = 0;
            for (int i = 0; i < allGameObjects.Length; i++)
            {
                if (TwoDimensions)
                {
                    var attatchedRigidBody2D = allGameObjects[i].GetComponent<Rigidbody2D>();

                    if (attatchedRigidBody2D)
                        MonoBehaviour.Destroy(attatchedRigidBody2D);

                    allGameObjects[i].gameObject.AddComponent<Rigidbody2D>();

                    if (allGameObjects[i].position.y < lowestYlevel)
                        lowestYlevel = allGameObjects[i].position.y;

                    var attatchedCollision2D = allGameObjects[i].GetComponent<Collider2D>();
                    if (attatchedCollision2D)
                    {
                        if (attatchedCollision2D.isTrigger)
                            attatchedCollision2D.isTrigger = false;
                    }
                    else
                        allGameObjects[i].gameObject.AddComponent<BoxCollider2D>();

                    continue;
                }

                var attatchedRigidBody = allGameObjects[i].GetComponent<Rigidbody>();
                if (attatchedRigidBody)
                    MonoBehaviour.Destroy(attatchedRigidBody);

                allGameObjects[i].gameObject.AddComponent<Rigidbody>();
                if (attatchedRigidBody.position.y < lowestYlevel)
                    lowestYlevel = attatchedRigidBody.position.y;

                var attatchedCollision = allGameObjects[i].GetComponent<Collider>();
                if (attatchedCollision)
                {
                    if (attatchedCollision.isTrigger)
                        attatchedCollision.isTrigger = false;
                }
                else
                    allGameObjects[i].gameObject.AddComponent<BoxCollider>();
            }

            if (TwoDimensions)
            {
                var stoppper = new GameObject("Stopper", typeof(StayAtPosition));
                stoppper.transform.position = new Vector3(0, lowestYlevel - 10);
                stoppper.transform.localScale = new Vector3(1000, 1);
                stoppper.AddComponent<BoxCollider2D>();
            }
            else
            {
                var stoppper = new GameObject("Stopper", typeof(StayAtPosition));
                stoppper.transform.position = new Vector3(0, lowestYlevel - 10, 0);
                stoppper.transform.localScale = new Vector3(1000, 1, 1000);
                stoppper.AddComponent<BoxCollider>();
            }

            return "deleted rigidbodies";
        }

        private enum MemberType : byte
        {
            Unknown,
            Field,
            Property,
            Method,
        }
    }
}
