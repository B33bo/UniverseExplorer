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

        private void ColorChanged(object val)
        {
            if (val is double)
                TargetStar.StarColor = GetStarColor();
            spriteRenderer.color = TargetStar.StarColor;
            worleyNoise.color = spriteRenderer.color;
            Glow.color = spriteRenderer.color;
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
            TargetStar.ColorChange += ColorChanged;

            TargetStar.StarColor = GetStarColor();

            //This is to do with sprite masks and sorting order
            //This happens so the weird texture doesn't clash with another star
            int sortingOrder = RandomNum.Get(0, 10000, Target.RandomNumberGenerator);
            spriteMask.frontSortingOrder = sortingOrder + 1;
            spriteMask.backSortingOrder = sortingOrder;
            worleyNoise.sortingOrder = sortingOrder;

            transform.localScale = Scale;

            SolarSystemSpawner.sun = Target as Star;

            if (SceneManager.GetActiveScene().name == "Galaxy")
                TargetStar.SpawnPlanets(transform);
            else
                TargetStar.planets = new PlanetRenderer[0];
        }

        public Color GetStarColor()
        {
            return colorGradient.Evaluate((float)(TargetStar.Temperature - Star.minTemp) / (Star.maxTemp - Star.minTemp));
        }

        public override void OnUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);
        }
    }
}
