using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public abstract class PlanetRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        protected SpriteRenderer sprite;

        public float SpawnWeight;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = (Planet)Activator.CreateInstance(PlanetType);
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            SpawnPlanet(pos, seed);
            LowResScale = 75;

            if (BodyManager.Parent is not Moon && (Target.Seed < 0 || Target.Seed > Star.Pluto))
                StartCoroutine(SpawnMoons());
        }

        private IEnumerator SpawnMoons()
        {
            yield return new WaitForEndOfFrame();

            (Target as Planet).SpawnMoons(transform);
        }

        public override void OnUpdate()
        {
            if (transform.parent is null)
                return;
            Debug.DrawLine(transform.position, transform.parent.position);
            transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);
        }

        protected override void HighRes()
        {
            if (sprite == null)
                return;
            sprite.enabled = true;
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            if (sprite == null)
                return;
            sprite.enabled = false;
        }

        public abstract Type PlanetType { get; }

        public abstract void SpawnPlanet(Vector2 pos, int? seed);
    }
}
