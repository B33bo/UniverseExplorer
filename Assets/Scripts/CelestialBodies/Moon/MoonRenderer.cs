using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe
{
    public class MoonRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform crater;

        [SerializeField]
        private SpriteRenderer sprite;

        private List<GameObject> craters = new();

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

            LowResScale = Scale.x * 15;

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

            craters.Add(crater.gameObject);
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name != "Galaxy")
                return;
            sprite.enabled = false;
            for (int i = 0; i < craters.Count; i++)
                craters[i].SetActive(false);
        }

        protected override void HighRes()
        {
            sprite.enabled = true;
            crater.gameObject.SetActive(true);
            for (int i = 0; i < craters.Count; i++)
                craters[i].SetActive(true);
        }
    }
}
