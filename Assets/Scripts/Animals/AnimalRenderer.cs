using System.Collections.Generic;
using UnityEngine;

namespace Universe.Animals
{
    public abstract class AnimalRenderer : CelestialBodyRenderer
    {
        public static List<int> seedsSpawned = new List<int>();
        public abstract System.Type AnimalType { get; }

        public override void Spawn(Vector2 pos, int? seed)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (transform.position.x < CameraControl.Instance.CameraBounds.xMin - 10 ||
                transform.position.x > CameraControl.Instance.CameraBounds.xMax + 10)
                Destroy(gameObject);
            OnUpdate();
        }

        public abstract void Spawn(Vector2 position, int? seed, Animal species);

        protected override void Destroyed()
        {
            seedsSpawned.Remove(Target.Seed);
        }
    }
}
