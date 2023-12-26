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

        private List<LineRenderer> lines = new();
        private float spawnTime;
        public Color color;

        protected override void OnAddPosition(Vector3 pos, LineRenderer line)
        {
            lines.Add(line);
        }

        public override void OnUpdate()
        {
            float alpha = Mathf.Clamp01(1 - (Time.time - spawnTime));

            for (int i = 0; i < lines.Count; i++)
            {
                Color start = color;
                start.a = alpha;
                lines[i].startColor = start;

                Color end = color;
                end.a = alpha;
                lines[i].endColor = end;
            }
            lavaLight.intensity = alpha;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);

            spawnTime = Time.time;

            audioSource.pitch = RandomNum.GetFloat(-.75f, 1.5f, Target.RandomNumberGenerator);
            audioSource.Play();

            lavaLight.color = color;
        }
    }
}
