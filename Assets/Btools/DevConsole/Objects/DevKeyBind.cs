using System.Collections.Generic;
using UnityEngine;

namespace Btools.DevConsole
{
    public class DevKeyBind : MonoBehaviour
    {
        public static List<(KeyCode, DevKeyBind)> Instances = new List<(KeyCode, DevKeyBind)>();
        public KeyCode key;
        public Type type;
        public string Command;

        public enum Type
        {
            Held,
            HeldFixedUpdate,
            Up,
            Down,
        }

        private void Start()
        {
            Instances.Add((key, this));
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (ExecuteCommand())
                Debug.Log(DevCommands.Execute(Command));
        }

        private void FixedUpdate()
        {
            if (type == Type.HeldFixedUpdate && Input.GetKey(key))
                Debug.Log(DevCommands.Execute(Command));
        }

        private bool ExecuteCommand()
        {
            switch (type)
            {
                case Type.Held:
                    return Input.GetKey(key);
                case Type.Up:
                    return Input.GetKeyUp(key);
                case Type.Down:
                    return Input.GetKeyDown(key);
                default:
                    return false;
            }
        }

        public void Unbind()
        {
            Destroy(gameObject);
        }
    }
}
