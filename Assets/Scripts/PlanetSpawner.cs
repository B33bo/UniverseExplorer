using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class PlanetSpawner : MonoBehaviour
    {
        private int seed;
        [SerializeField]
        private MoonRenderer moonRenderer;

        private void Awake()
        {
            if (BodyManager.Parent is null)
                BodyManager.Parent = new ErrorPlanet();

            seed = BodyManager.Parent.Seed;
            Planet planet = BodyManager.Parent as Planet;
            var prefab = Resources.Load<CelestialBodyRenderer>(planet.ObjectFilePos);

#if UNITY_EDITOR
            if (prefab is null)
                Debug.LogError("Error loading Assest/Resources/" + planet.ObjectFilePos);
#endif

            CelestialBodyRenderer celestialBodyRenderer = Instantiate(prefab);
            celestialBodyRenderer.Spawn(Vector2.zero, null);

            var rand = new System.Random(seed);
            Moon[] moons = new Moon[rand.Next(0, 5)];

            for (int i = 0; i < moons.Length; i++)
            {
                var newMoonRenderer = Instantiate(moonRenderer);
                newMoonRenderer.SpawnMoon(RandomNum.GetFloat(2, 10, rand), rand.Next(), RandomNum.GetFloat(0, 10, rand));
                moons[i] = newMoonRenderer.Target as Moon;
                moons[i].planet = planet;
            }

            //planet.moons = moons;
        }
    }
}
