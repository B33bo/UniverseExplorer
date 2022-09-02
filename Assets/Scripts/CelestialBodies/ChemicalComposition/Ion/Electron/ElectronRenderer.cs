using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class ElectronRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Orbiter orbiter;

        private bool doOrbiting = false;

        public bool IsAntimatter = false;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Electron();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            if (!doOrbiting)
                Destroy(orbiter);

            if (IsAntimatter)
            {
                (Target as Electron).Antimatter();
                GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "+";
            }
        }

        public void InitOrbiter(float radius, float initial)
        {
            doOrbiting = true;
            orbiter.Activate(initial, radius, 2);
        }
    }
}
