using UnityEngine;

namespace Universe.CelestialBodies
{
    public class OreGenerator : MonoBehaviour
    {
        public static Ore[] ores;
        public static float[] weights;
        public static float weightTotal;

        [SerializeField]
        private int min = 1, max = 6;

        private void Awake()
        {
            System.Random rand = new(BodyManager.GetSeed());

            ores = new Ore[rand.Next(min, max)];
            weights = new float[ores.Length];
            weightTotal = 0;

            for (int i = 0; i < ores.Length; i++)
            {
                Ore ore = new();
                string name = RandomNum.GetWord(RandomNum.Get(1, 3, rand), rand);

                if (rand.Next(4) == 0)
                    name += "ite";

                ore.Create(Vector2.zero);
                ore.Load(name, "", RandomNum.GetColor(rand));
                ores[i] = ore;

                float weight = (float)rand.NextDouble();
                weightTotal += weight;
                weights[i] = weight;
            }
        }
    }
}
