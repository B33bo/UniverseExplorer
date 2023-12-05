using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class StarRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private SpriteRenderer worleyNoise;

        [SerializeField]
        private Light2D starLight;

        private void ColorChanged(Variable val)
        {
            Star star = Target as Star;
            if (val == null || val.VariableName == "Temperature")
                star.ResetColor();
            spriteRenderer.color = star.StarColor;
            worleyNoise.color = spriteRenderer.color;
            worleyNoise.material.SetColor("_Color", star.StarColor);
            starLight.color = star.StarColor;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Star star = new();
            Target = star;

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            Scale = GetFairSizeCurve((Target as Star).trueRadius, 4f, 1.2f) * Vector3.one;
            star.OnInspected += ColorChanged;
            ColorChanged(null);

            transform.localScale = Scale;
            LowResScale = Scale.x * 10;

            SolarSystemSpawner.sun = Target as Star;

            if (SceneManager.GetActiveScene().name == "Galaxy")
                star.SpawnPlanets(transform);
            else
            {
                star.planets = new PlanetRenderer[0];
                starLight.enabled = false;
            }
        }

        protected override void HighRes()
        {
            worleyNoise.enabled = true;
        }

        protected override void LowRes()
        {
            if (SceneManager.GetActiveScene().name == "Galaxy")
            {
                worleyNoise.enabled = false;
            }
        }

        public override void OnUpdate()
        {
            transform.rotation = GlobalTime.Rotation;
        }
    }
}
