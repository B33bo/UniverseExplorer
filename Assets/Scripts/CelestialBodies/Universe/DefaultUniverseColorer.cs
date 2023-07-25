using UnityEngine;

namespace Universe
{
    [CreateAssetMenu(menuName = "SpaceGame/UniverseColorer", fileName = "UniverseColorer")]
    public class DefaultUniverseColorer : ScriptableObject
    {
        private static DefaultUniverseColorer _inst;
        public static DefaultUniverseColorer Instance
        {
            get
            {
                _inst = _inst != null ? _inst : Resources.Load<DefaultUniverseColorer>("DefaultUniverseColorer");
                return _inst;
            }
        }

        public Gradient BlackBodyRadiation;
    }
}
