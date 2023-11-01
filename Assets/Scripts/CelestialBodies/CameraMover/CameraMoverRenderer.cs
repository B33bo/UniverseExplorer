using UnityEngine;

namespace Universe.CelestialBodies
{
    public class CameraMoverRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new CameraMover();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            StartCoroutine(DestroyAfterTime());
        }

        private System.Collections.IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
            cameraLerpTarget = null;
        }
    }
}
