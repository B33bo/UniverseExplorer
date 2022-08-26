using UnityEngine;
using Universe.CelestialBodies.Biomes.Snow;

namespace Universe
{
    public class SnowmanRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform leftArm, rightArm;

        [SerializeField]
        private Transform leftEye, rightEye;

        [SerializeField]
        private Transform nose;

        [SerializeField]
        private Transform[] mouth;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Snowman();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            leftArm.rotation = Quaternion.Euler(0, 0, (float)RandomNum.Get(-45, 135f, Target.RandomNumberGenerator));
            rightArm.rotation = Quaternion.Euler(0, 0, (float)RandomNum.Get(240, 300, Target.RandomNumberGenerator));

            leftEye.rotation = Quaternion.Euler(0, 0, RandomNum.Get(0, 360, Target.RandomNumberGenerator));
            rightEye.rotation = Quaternion.Euler(0, 0, RandomNum.Get(0, 360, Target.RandomNumberGenerator));

            leftEye.position += new Vector3((float)RandomNum.Get(-.2, .2, Target.RandomNumberGenerator), (float)RandomNum.Get(-.2, .2, Target.RandomNumberGenerator));
            rightEye.position += new Vector3((float)RandomNum.Get(-.2, .2, Target.RandomNumberGenerator), (float)RandomNum.Get(-.2, .2, Target.RandomNumberGenerator));

            for (int i = 0; i < mouth.Length; i++)
            {
                mouth[i].SetPositionAndRotation(
                    mouth[i].transform.position + new Vector3((float)RandomNum.Get(-.05, .05, Target.RandomNumberGenerator), (float)RandomNum.Get(-.05, .05, Target.RandomNumberGenerator)),
                    Quaternion.Euler(0, 0, RandomNum.Get(0, 360, Target.RandomNumberGenerator)));
            }
        }
    }
}
