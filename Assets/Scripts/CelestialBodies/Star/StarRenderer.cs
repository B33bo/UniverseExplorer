using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class StarRenderer : CelestialBodyRenderer
    {
        private Star TargetStar => Target as Star;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private SpriteRenderer worleyNoise;

        [SerializeField]
        private SpriteRenderer Glow;

        private void ColorChanged(Variable val)
        {
            if (val == null || val.VariableName == "Temperature")
                TargetStar.StarColor = GetStarColor();
            spriteRenderer.color = TargetStar.StarColor;
            worleyNoise.color = spriteRenderer.color;
            Glow.color = spriteRenderer.color;
            worleyNoise.material.SetColor("_Color", TargetStar.StarColor);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Star();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            Scale = GetFairSizeCurve((Target as Star).trueRadius, 4f, 1.2f) * Vector3.one;
            TargetStar.OnInspected += ColorChanged;
            ColorChanged(null);

            TargetStar.StarColor = GetStarColor();

            transform.localScale = Scale;

            SolarSystemSpawner.sun = Target as Star;

            if (SceneManager.GetActiveScene().name == "Galaxy")
                TargetStar.SpawnPlanets(transform);
            else
                TargetStar.planets = new PlanetRenderer[0];
        }

        public Color GetStarColor()
        {
            return Universe.LoadedInfo.BlackBodyRadiation.Evaluate((float)(TargetStar.Temperature - Star.minTemp) / (Star.maxTemp - Star.minTemp));
        }

        public override void OnUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);
        }
    }
}
