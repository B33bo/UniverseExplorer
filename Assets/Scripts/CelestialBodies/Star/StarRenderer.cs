using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe.CelestialBodies.Planets
{
    public class StarRenderer : CelestialBodyRenderer
    {
        private Star TargetStar => Target as Star;

        public Gradient colorGradient;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private SpriteRenderer worleyNoise;

        [SerializeField]
        private SpriteRenderer Glow;

        [SerializeField]
        private SpriteMask spriteMask;

        private void Start()
        {
            if (BodyManager.Parent is Star)
            {
                Spawn(Vector2.zero, BodyManager.Parent.Seed);
                LoadWorley(128, GetStarColor(), RandomNum.Get(10, 55, Target.RandomNumberGenerator));
            }
            else if (SceneManager.GetActiveScene().name == "SolarSystem")
            {
                Spawn(Vector2.zero, null);
                LoadWorley(128, GetStarColor(), RandomNum.Get(10, 55, Target.RandomNumberGenerator));
            }
            else
            {
                worleyNoise.color = GetStarColor();
            }

            SolarSystemSpawner.sun = Target as Star;

            if (SceneManager.GetActiveScene().name == "Galaxy")
                TargetStar.SpawnPlanets(transform);
            else
                TargetStar.planets = new PlanetRenderer[0];
        }

        private void LoadWorley(int size, Color color, int pointCount)
        {
            var texture = NoiseGenerator.WorleyNoise(Target.Seed, pointCount, size, color, new Color(0, 0, 0, .7f));

            worleyNoise.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            worleyNoise.transform.localScale = 104f / size * Vector3.one;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Star();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            Scale = GetFairSizeCurve((Target as Star).trueRadius, 4f, 1.2f) * Vector3.one;

            TargetStar.starColor = GetStarColor();

            spriteRenderer.color = TargetStar.starColor;
            Glow.color = spriteRenderer.color;

            //This is to do with sprite masks and sorting order
            //This happens so the weird texture doesn't clash with another star
            int sortingOrder = RandomNum.Get(0, 10000, Target.RandomNumberGenerator);
            spriteMask.frontSortingOrder = sortingOrder + 1;
            spriteMask.backSortingOrder = sortingOrder;
            worleyNoise.sortingOrder = sortingOrder;

            transform.localScale = Scale;
        }

        public Color GetStarColor()
        {
            return colorGradient.Evaluate((float)(TargetStar.Temperature - 3000) / 7000);
        }

        public override void OnUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);
        }

        protected override void Destroyed()
        {
            return;
            var planets = TargetStar.planets;
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i].IsDestroyed)
                    continue;
                Destroy(planets[i].gameObject);
            }
        }
    }
}
