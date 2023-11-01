using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.CelestialBodies;

namespace Universe
{
    public class SkyBlackholeRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {

        }

        public void Spawn(BlackHole b)
        {
            Target = b;
            Target.Position = new Vector3(0, 10);
        }

        private void OnMouseDown()
        {
            if (SceneManager.sceneCount > 1)
                return;

            ObjectDataLoader.celestialBody = Target;
            SceneManager.LoadScene("ObjectData", LoadSceneMode.Additive);
        }
    }
}
