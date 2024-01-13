using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class ToxicPlanetRenderer : PlanetRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        public override Type PlanetType => typeof(ToxicPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            ToxicPlanet toxicPlanet = new ToxicPlanet();
            Target = toxicPlanet;

            Scale = (float)GetFairSize(Target.Width, ToxicPlanet.MinScale, ToxicPlanet.MaxScale) * 5 * Vector2.one;

            if (seed.HasValue)
                toxicPlanet.SetSeed(seed.Value);

            toxicPlanet.OnInspected += ColorReset;
            toxicPlanet.Create(pos);

            var tree = ShapeMaker.GetRandomWebLines(Target.RandomNumberGenerator);
            Mesh mesh = TreeNodeMeshDrawer.GenerateMesh(tree);
            meshFilter.mesh = mesh;
            meshRenderer.material.color = toxicPlanet.ToxicColor;
        }

        private void ColorReset(Variable v)
        {
            if (v.VariableName == "Type")
                (Target as ToxicPlanet).RefreshColor();
            Color c = (Target as ToxicPlanet).ToxicColor;

            meshRenderer.material.color = c;
        }
    }
}
