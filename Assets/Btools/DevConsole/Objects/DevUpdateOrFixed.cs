using UnityEngine;
using System.Collections.Generic;

namespace Btools.DevConsole
{
    public class DevUpdateOrFixed : MonoBehaviour
    {
        public static List<DevUpdateOrFixed> Instances = new List<DevUpdateOrFixed>();
        public enum Type
        {
            Update,
            FixedUpdate,
            Start,
            Awake,
        }

        public Type type;
        public string Command;

        private void Start()
        {
            DontDestroyOnLoad(this);
            Instances.Add(this);
            if (type == Type.Start)
                Debug.Log(DevCommands.Execute(Command));
        }

        private void Update()
        {
            if (type == Type.Update)
                DevCommands.Execute(Command);
        }

        private void FixedUpdate()
        {
            if (type == Type.FixedUpdate)
                Debug.Log(DevCommands.Execute(Command));
        }

        public void Unbind()
        {
            Destroy(gameObject);
        }
    }
}
