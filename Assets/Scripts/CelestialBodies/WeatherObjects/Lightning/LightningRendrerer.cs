using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universe.CelestialBodies.Planets.Molten;

namespace Universe
{
    public class LightningRendrerer : LavaForkRenderer
    {
        [SerializeField]
        private AudioSource audioSource;

        private float spawnTime;
        private Material material;
        public Color color;

        public override void OnUpdate()
        {
            float alpha = Mathf.Clamp01(1 - (Time.time - spawnTime));
            Color newColor = color;
            newColor.a = alpha;

            material.color = newColor;
            lavaLight.intensity = alpha;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);

            spawnTime = Time.time;

            audioSource.pitch = RandomNum.GetFloat(-.75f, 1.5f, Target.RandomNumberGenerator);
            audioSource.Play();

            material = meshRenderer.material;
            material.color = color;
            lavaLight.color = color;
        }
    }
}
