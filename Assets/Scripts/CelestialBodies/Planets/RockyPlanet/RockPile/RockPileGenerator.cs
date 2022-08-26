using UnityEngine;
using Universe.CelestialBodies.Planets.Rocky;

namespace Universe
{
    public class RockPileGenerator : MonoBehaviour
    {
        [SerializeField]
        private BoulderRenderer prefab;

        public void Start()
        {
            if (!(BodyManager.Parent is RockPile rockPile))
            {
                rockPile = new RockPile();
                rockPile.SetSeed(0);
                rockPile.Create(Vector2.zero);
            }

            float pos = 0;
            for (int i = 0; i < rockPile.Rocks; i++)
            {
                var newBoulder = Instantiate(prefab, new Vector3(0, pos), Quaternion.identity);
                newBoulder.SpawnBoulder(new Vector2(0, pos), rockPile[i], rockPile.colors[i], i);
                pos += 2;
            }
        }
    }
}
