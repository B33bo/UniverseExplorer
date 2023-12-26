using UnityEngine;

namespace Universe.CelestialBodies
{
    public class BlackHoleRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private BlackHoleAccretionDiskRenderer[] orbiters;

        private RingObject[] ringObjects;

        [SerializeField]
        private Vector2 ringScale;

        private Vector2 elispeScale;

        [SerializeField]
        private bool Clockwise;
        private float Orbitspeed;

        [SerializeField]
        private bool Supermassive = false;

        [SerializeField]
        private Transform discParticle;

        [SerializeField]
        private ParticleSystem glowDisc;

        private struct RingObject
        {
            public float Offset;
            public float RotationOffset;
            public SpriteRenderer spriteRenderer;
            public BlackHoleAccretionDisk blackHoleAccretionDisk;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            double min, max;
            float multiplier = 1;

            if (Supermassive)
            {
                Target = new SupermassiveBlackHole();
                min = SupermassiveBlackHole.MinScale;
                max = SupermassiveBlackHole.MaxScale;
                multiplier = 3;
            }
            else
            {
                Target = new BlackHole();
                min = BlackHole.MinScale;
                max = BlackHole.MaxScale;
            }

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
            Scale = GetFairSize((float)Target.Width, (float)min, (float)max) * multiplier * Vector2.one;

            LowResScale = Scale.x * 60;
            discParticle.transform.localScale *= Scale;

            transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(-45f, 45f, Target.RandomNumberGenerator));
            transform.localScale = Scale * Vector2.one;

            Orbitspeed = RandomNum.GetFloat(.5f, 3, Target.RandomNumberGenerator);
            Clockwise = RandomNum.GetBool(Target.RandomNumberGenerator);

            SpawnRings();

            elispeScale.x = 1 / ringScale.x;
            elispeScale.y = 1 / ringScale.y;
        }

        private void SpawnRings()
        {
            ringObjects = new RingObject[orbiters.Length];
            for (int i = 0; i < ringObjects.Length; i++)
            {
                float offsetPercent = ((float)i / ringObjects.Length) + RandomNum.GetFloat(.1f, Target.RandomNumberGenerator);
                orbiters[i].Spawn(Vector2.zero, Target.Seed + i, Target as BlackHole);

                ringObjects[i] = new RingObject()
                {
                    Offset = offsetPercent * 2 * Mathf.PI,
                    RotationOffset = (orbiters[i].Target as BlackHoleAccretionDisk).rotationOffset,
                    spriteRenderer = orbiters[i].GetComponent<SpriteRenderer>(),
                    blackHoleAccretionDisk = orbiters[i].Target as BlackHoleAccretionDisk,
                };
            }
        }

        protected override void LowRes()
        {
            glowDisc.Stop();
            for (int i = 0; i < ringObjects.Length; i++)
                orbiters[i].enabled = false;
        }

        protected override void HighRes()
        {
            glowDisc.Play();
            for (int i = 0; i < ringObjects.Length; i++)
                orbiters[i].enabled = true;
        }

        private void Start()
        {
            if (Target is null)
                Spawn(transform.position, null);
        }

        public override void OnUpdate()
        {
            if (IsLowRes)
                return;

            for (int i = 0; i < ringObjects.Length; i++)
                SetPos(ringObjects[i], Time.time * Orbitspeed, elispeScale, Clockwise);
        }

        private void SetPos(RingObject ringObject, float t, Vector2 elipseScale, bool clockwise)
        {
            const float rotateSpeed = 40;

            t += ringObject.Offset;

            if (Clockwise)
                ringObject.spriteRenderer.sortingOrder = t % (Mathf.PI * 2) < Mathf.PI ? 1 : -1;
            else
                ringObject.spriteRenderer.sortingOrder = t % (Mathf.PI * 2) < Mathf.PI ? -1 : 1;

            if (clockwise)
                t *= -1;

            float newRot = t * rotateSpeed + ringObject.RotationOffset;

            Vector2 newPosition = new(
                Mathf.Cos(t) / elipseScale.x,
                Mathf.Sin(t) / elipseScale.y);

            ringObject.blackHoleAccretionDisk.Position = newPosition;
            ringObject.spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, newRot);
        }
    }
}
