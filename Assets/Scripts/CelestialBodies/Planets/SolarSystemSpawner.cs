using UnityEngine;
using System.Collections;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class SolarSystemSpawner : MonoBehaviour
    {
        private int seed;
        private System.Random rnd;

        [SerializeField]
        private float impossibleDistance, hotDistance, goldilocksDistance;

        [SerializeField]
        private CelestialBodyRenderer[] hot, goldilocks, cold;

        private float diameter;
        public static Star sun;

        private IEnumerator Start()
        {
            yield return new WaitForFrames(1);

            BodyManager.ReloadCommands();
            BodyManager.InvokeSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

            seed = BodyManager.GetSeed();
            rnd = new System.Random(seed);

            diameter = RandomNum.GetFloat(20, 60, rnd);

            hotDistance *= diameter;
            goldilocksDistance *= diameter;

            int planets = rnd.Next(0, 15);
            for (int i = 0; i < planets; i++)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            float radius = diameter;

            float orbitalRadius = RandomNum.GetFloat(radius, rnd);
            var prefab = GetPrefabFromRadius(orbitalRadius);

            if (prefab is null)
                return;

            var newObj = Instantiate(prefab, new Vector3(orbitalRadius, 0), Quaternion.identity);
            newObj.Spawn(new Vector2(orbitalRadius, 0), rnd.Next());

            if (newObj.Target is Planet p)
            {
                p.sun = sun;
            }
        }

        private CelestialBodyRenderer GetPrefabFromRadius(float radius)
        {
            if (radius > goldilocksDistance)
                return cold[rnd.Next(0, cold.Length)];
            if (radius > hotDistance)
                return goldilocks[rnd.Next(0, goldilocks.Length)];
            if (radius > impossibleDistance)
                return hot[rnd.Next(0, hot.Length)];
            return null;
        }
    }
}
