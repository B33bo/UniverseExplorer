using UnityEngine;

namespace Universe.CelestialBodies
{
    public class BlackHoleRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer[] accretionDiscs;

        private AccretionObject[] AccretionDiscObjects;

        [SerializeField]
        private Vector2 AccretionDiscScale;

        private Vector2 AccretionDiscScaleInverted;

        [SerializeField]
        private bool Clockwise;
        private float Orbitspeed;

        [SerializeField]
        private bool Supermassive = false;

        [SerializeField]
        private Transform discParticle;

        public override void Spawn(Vector2 pos, int? seed)
        {
            if (Supermassive)
            {
                Target = new SupermassiveBlackHole();

                if (seed.HasValue)
                    Target.SetSeed(seed.Value);

                Target.Create(pos);
                Scale = GetFairSize((float)Target.Radius, (float)SupermassiveBlackHole.MinScale, (float)SupermassiveBlackHole.MaxScale) * 5 * Vector2.one;
            }
            else
            {
                Target = new BlackHole();

                if (seed.HasValue)
                    Target.SetSeed(seed.Value);

                Target.Create(pos);
                Scale = GetFairSize((float)Target.Width, (float)BlackHole.MinScale, (float)BlackHole.MaxScale) * Vector2.one;
            }

            discParticle.transform.localScale *= Scale;

            transform.rotation = Quaternion.Euler(0, 0, RandomNum.GetFloat(-45f, 45f, Target.RandomNumberGenerator));
            transform.localScale = Scale * Vector2.one;
            Orbitspeed = RandomNum.GetFloat(.5f, 3, Target.RandomNumberGenerator);
            Clockwise = RandomNum.GetBool(Target.RandomNumberGenerator);

            AccretionDiscObjects = new AccretionObject[accretionDiscs.Length];
            for (int i = 0; i < AccretionDiscObjects.Length; i++)
            {
                AccretionDiscObjects[i] = new AccretionObject();
                AccretionDiscObjects[i].Offset = ((float)i / AccretionDiscObjects.Length) + RandomNum.GetFloat(.25f, Target.RandomNumberGenerator);
                AccretionDiscObjects[i].RotationOffset = RandomNum.GetFloat(360, Target.RandomNumberGenerator);
                AccretionDiscObjects[i].spriteRenderer = accretionDiscs[i];

                AccretionDiscObjects[i].Init();
            }

            AccretionDiscScaleInverted.x = 1 / AccretionDiscScale.x;
            AccretionDiscScaleInverted.y = 1 / AccretionDiscScale.y;
        }

        private void Start()
        {
            if (Target is null)
                Spawn(transform.position, null);
        }

        private void Update()
        {
            for (int i = 0; i < AccretionDiscObjects.Length; i++)
                AccretionDiscObjects[i].SetPos(Time.time * Orbitspeed, AccretionDiscScaleInverted, Clockwise);
        }
    }
}
