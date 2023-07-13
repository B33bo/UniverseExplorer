using UnityEngine;
using Universe.CelestialBodies.Biomes.Desert;

namespace Universe
{
    public class CactusRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform leftarm, rightarm;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Cactus();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            leftarm.localPosition = new Vector3(leftarm.localPosition.x, (float)RandomNum.Get(-.1, .75, Target.RandomNumberGenerator));
            rightarm.localPosition = new Vector3(rightarm.localPosition.x, (float)RandomNum.Get(-.1, .75, Target.RandomNumberGenerator));

            Scale = new Vector3((float)Target.Width, (float)Target.Height) / (float)Measurement.M;
        }
    }
}
