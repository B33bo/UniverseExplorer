using UnityEngine;
using System.Collections.Generic;

namespace Universe
{
    public class GlobalObject : MonoBehaviour
    {
        private static readonly List<string> objectsSpawned = new List<string>();

        private void Awake()
        {
            if (objectsSpawned.Contains(name))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            objectsSpawned.Add(name);
        }
    }
}
