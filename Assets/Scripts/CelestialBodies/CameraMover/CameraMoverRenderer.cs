using Btools.DevConsole;
using UnityEngine;

namespace Universe.CelestialBodies
{
    public class CameraMoverRenderer : CelestialBodyRenderer
    {
        private static bool Selectable;
        public override void Spawn(Vector2 pos, int? seed)
        {
            GetComponent<BoxCollider2D>().enabled = Selectable;

            Target = new CameraMover();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            StartCoroutine(DestroyAfterTime());
            DevCommands.RegisterVar(new DevConsoleVariable("CameraMoverSelectable", "Is the camera mover selectable", typeof(bool),
                () => Selectable,
                x => Selectable = bool.Parse(x)));
        }

        private System.Collections.IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
            cameraLerpTarget = null;
        }
    }
}
