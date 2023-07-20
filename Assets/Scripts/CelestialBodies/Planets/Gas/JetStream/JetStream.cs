using UnityEngine;

namespace Universe.CelestialBodies.Planets.Gas
{
    public class JetStream : MonoBehaviour
    {
        public int Seed;

        [SerializeField]
        private ParticleSystem jetParticles;

        private ParticleSystem.MainModule main;

        private void Start()
        {
            System.Random randomNumGenerator = new System.Random(Seed);

            float size = (float)RandomNum.Get(8.0, 15.0, randomNumGenerator);

            main = jetParticles.main;
            main.startSize = size;

            float lerp = RandomNum.GetFloat(1, randomNumGenerator);
            Color color = Color.Lerp(ColorHighlights.Instance.primary, ColorHighlights.Instance.secondary, lerp);

            var startColor = new ParticleSystem.MinMaxGradient(color);
            main.startColor = startColor;

            float speed = (float)RandomNum.Get(10, 25f, randomNumGenerator);
            main.startSpeed = speed;

            jetParticles.GetComponent<ParticleSystemRenderer>().sortingOrder = RandomNum.Get(int.MinValue, int.MaxValue, randomNumGenerator);
            jetParticles.randomSeed = (uint)Seed;
            jetParticles.Play();
            CameraControl.Instance.OnPositionUpdate += UpdatePosition;
        }

        private void UpdatePosition(Rect cameraRect)
        {
            transform.position = new Vector3(cameraRect.xMin - 2, transform.position.y);

            float time = cameraRect.width / main.startSpeed.constantMax;
            jetParticles.Simulate(time);
            jetParticles.Play();

            main.startLifetime = time;
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= UpdatePosition;
        }
    }
}
