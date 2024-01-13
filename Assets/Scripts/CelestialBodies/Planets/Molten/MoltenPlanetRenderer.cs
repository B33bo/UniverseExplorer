using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets
{
    public class MoltenPlanetRenderer : PlanetRenderer
    {
        private bool LerpDirection;
        private float LerpAmount;

        [SerializeField]
        private Color lerpStart, lerpEnd;

        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        public override Type PlanetType => typeof(MoltenPlanet);

        public override void SpawnPlanet(Vector2 pos, int? seed)
        {
            Scale = GetFairSize((float)Target.Width, (float)MoltenPlanet.MinScale, (float)MoltenPlanet.MaxScale) * Vector2.one;

            GenerateLines();
        }

        private void GenerateLines()
        {
            var tree = ShapeMaker.GetRandomWebLines(Target.RandomNumberGenerator);
            Mesh mesh = TreeNodeMeshDrawer.GenerateMesh(tree, TreeNodeMeshDrawer.UVMode.DepthBased);
            meshFilter.mesh = mesh;
        }

        public override void OnUpdate()
        {
            int BackgroundDirectionInt = LerpDirection ? 1 : -1;

            LerpAmount += Time.deltaTime * BackgroundDirectionInt;

            if (LerpAmount > 1 || LerpAmount < 0)
                LerpDirection = !LerpDirection;

            //Color newColor = Color.Lerp(lerpStart, lerpEnd, LerpAmount);
            //meshRenderer.material.color = newColor;
        }
    }
}
