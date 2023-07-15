using UnityEngine;

namespace Universe.Blocks
{
    public class WaveRenderer : CelestialBodyRenderer
    {
        private const float waveExtension = 3;
        private const float waveTrough = 20;
        private const float speed = .2f;

        public override void Spawn(Vector2 pos, int? seed)
        {
            pos.y -= .5f;
            Wave wave = new Wave();
            Target = wave;

            if (seed.HasValue)
                wave.SetSeed(seed.Value);
            wave.Create(pos);
        }

        public override void OnUpdate()
        {
            float sinValue = Mathf.Sin(GlobalTime.Time * speed + Target.Position.x);
            sinValue = (sinValue + 1) * .5f; // normalize between 0-1 insetead of -1 - 1
            float scale = sinValue * waveExtension + waveTrough - Target.Position.y;
            Target.Height = scale * Measurement.M;

            transform.localScale = new Vector3(1, scale);
        }
    }
}
