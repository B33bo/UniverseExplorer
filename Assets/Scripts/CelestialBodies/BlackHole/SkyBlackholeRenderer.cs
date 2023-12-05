using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.CelestialBodies;

namespace Universe
{
    public class SkyBlackholeRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new BlackHole();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
            CameraFocus = false;
        }

        public void Spawn(BlackHole b)
        {
            Target = b;
            Target.Position = new Vector3(0, 10);
            CameraFocus = false;
        }
    }
}
