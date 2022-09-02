using UnityEngine;

namespace Universe
{
    public class PeriodicTableTesting : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log(new Chemical(1, "H2-7O-2")[1].NumberofElectrons);
        }
    }
}
