using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Universe.CelestialBodies
{
    public class BlackHoleAccretionDiskRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private BlackHoleRenderer blackHoleRenderer;

        [SerializeField]
        private Collider2D collision;

        private IEnumerator Start()
        {
            yield return new WaitForFrames(4);
            if (Target is null)
                Spawn(Vector2.zero, (int)transform.position.x);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new BlackHoleAccretionDisk();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
            name = Target.Name;

            if (blackHoleRenderer is null)
                return;
            (Target as BlackHoleAccretionDisk).blackHole = blackHoleRenderer.Target as BlackHole;
        }

        public void Spawn(Vector2 pos, int? seed, BlackHole blackHole)
        {
            Spawn(pos, seed);
            (Target as BlackHoleAccretionDisk).blackHole = blackHole;
        }

        //overrides base.Update
        private void Update()
        {
            collision.enabled = CameraControl.Instance.MyCamera.orthographicSize < 25;
        }
    }
}
