using UnityEngine;

namespace Universe
{
    [CreateAssetMenu(fileName = "Gradient Container", menuName = "Universe/Gradient")]
    public class GradientContainer : ScriptableObject
    {
        public Gradient gradient;

        public static implicit operator Gradient(GradientContainer gradientContainer)
        {
            return gradientContainer.gradient;
        }
    }
}
