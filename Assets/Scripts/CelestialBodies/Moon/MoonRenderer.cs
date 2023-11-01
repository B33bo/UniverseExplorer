using UnityEngine;

namespace Universe
{
    public class MoonRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform crater;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Moon();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            Scale = GetFairSize((float)Target.Radius, 1000, 3000) * Vector2.one;

            crater.localPosition = new Vector3(
                (float)Target.RandomNumberGenerator.NextDouble(),
                (float)Target.RandomNumberGenerator.NextDouble()).normalized - new Vector3(.5f, .5f);

            int spriteMaskID = Target.RandomNumberGenerator.Next(0, short.MaxValue);
            var spriteMask = GetComponent<SpriteMask>();
            spriteMask.frontSortingOrder = spriteMaskID + 1;
            spriteMask.backSortingOrder = spriteMaskID - 1;

            crater.GetComponent<SpriteRenderer>().sortingOrder = spriteMaskID;

            int craterCount = RandomNum.Get(1, 15, Target.RandomNumberGenerator);

            for (int i = 1; i < craterCount; i++)
                AddNewCrater();
        }

        public void SpawnMoon(float orbitRadius, int seed, float orbitSpeed)
        {
            Spawn(new Vector2(orbitSpeed, 0), seed);
            GetComponent<Orbiter>().Activate(RandomNum.GetFloat(0, 360, Target.RandomNumberGenerator), orbitRadius, orbitSpeed);
        }

        private void AddNewCrater()
        {
            Vector3 movement = new Vector3(
                RandomNum.GetFloat(-.5f, .5f, Target.RandomNumberGenerator),
                RandomNum.GetFloat(-.5f, .5f, Target.RandomNumberGenerator));

            var newCrater = Instantiate(crater, transform);
            newCrater.localPosition = crater.localPosition;
            newCrater.localPosition += movement;// crater.localPosition + movement;

            if (newCrater.localPosition.magnitude > .5f)
                newCrater.localPosition = movement;
            crater = newCrater;
        }
    }
}
