using System.Collections.Generic;
using UnityEngine;

namespace Universe
{
    public class GlobalObject : MonoBehaviour
    {
        public static Dictionary<string, GameObject> GlobalObjects = new Dictionary<string, GameObject>();

        public string ID;

        public string[] Replace;

        private void Awake()
        {
            if (GlobalObjects.ContainsKey(ID))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            GlobalObjects.Add(ID, gameObject);

            for (int i = 0; i < Replace.Length; i++)
            {
                GlobalObjects.Remove(Replace[i]);
            }
        }
    }
}
