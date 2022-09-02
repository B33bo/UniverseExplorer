using UnityEngine;
using Universe.CelestialBodies.Planets.Water;

namespace Universe
{
    public class SandGrainRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private PolygonCollider2D collision;

        public static readonly ColorHSV[] Colors = new ColorHSV[]
        {
            new ColorHSV(60 / 360f, .68f, 1),    //yellow
            new ColorHSV(0, 0, .7f),             //white
            new ColorHSV(30 / 360f, 1, 1),       //orange
            new ColorHSV(0, 0, .2f),             //black
            new ColorHSV(0, .9f, 1),             //red
            new ColorHSV(120 / 360f, .75f, .75f),//green
            new ColorHSV(180 / 360f, .5f, 1),    //cyan
            //random
        };

        public override void Spawn(Vector2 pos, int? seed)
        {
            SandGrain sandGrain = new SandGrain();
            Target = sandGrain;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            meshFilter.mesh = sandGrain.mesh;
            collision.points = sandGrain.mesh.vertices.ToVector2();

            if (sandGrain.color == SandGrain.Color.Random)
            {
                meshRenderer.material.color = new ColorHSV(RandomNum.GetFloat(0, 1, Target.RandomNumberGenerator), .85f, 1);
                return;
            }

            ColorHSV selectedColor = Colors[(int)sandGrain.color];
            selectedColor.h += RandomNum.GetFloat(-.05f, .05f, Target.RandomNumberGenerator);
            selectedColor.s += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);
            selectedColor.v += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);
            meshRenderer.material.color = selectedColor;
        }
    }
}
