using UnityEngine;
using Universe.CelestialBodies.Planets.Error;

namespace Universe
{
    public class EndIsNighRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private bool positive;

        [SerializeField]
        private bool create;

        private void Start()
        {
            if (create)
                Spawn(new Vector2(positive ? EndIsNighSign.TheEnd : -EndIsNighSign.TheEnd, 4), null);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new EndIsNighSign();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }
    }
}
