using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class ChemicalCompositionSpawner : Spawner
    {
        public static ChemicalComposition Composition = ChemicalComposition.None;

        public override void OnStart()
        {
            base.OnStart();

            weights = new float[Composition.weights.Length + 1];
            objects = new CelestialBodyRenderer[weights.Length + 1];

            for (int i = 0; i < Composition.chemicals.Length; i++)
            {
                Debug.Log("Objects/Compounds/" + Composition.chemicals[i].ToString());
                objects[i + 1] = Resources.Load<CelestialBodyRenderer>("Objects/Compounds/" + Composition.chemicals[i].ToString());
                weights[i + 1] = Composition.weights[i];
            }
        }
    }
}
