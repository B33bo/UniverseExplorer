using UnityEngine;

namespace Universe
{
    public class ColorHighlights : MonoBehaviour
    {
        public static ColorHighlights Instance { get; private set; }

        public Color primary, secondary, tertiary;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
