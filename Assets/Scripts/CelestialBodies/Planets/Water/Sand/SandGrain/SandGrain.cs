using UnityEngine;
using System.Collections.Generic;

namespace Universe.CelestialBodies.Planets.Water
{
    public class SandGrain : CelestialBody
    {
        public override string TypeString => "Sand Grain";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Mesh mesh;

        public static float[] ColorWeightChars = new float[]
        {
            50, //yellow
            20, //white
            10, //orange
            5,  //red
            5,  //black
            5,  //green
            4,  //cyan
            1,  //random color
        };

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Sand Grain";
            mesh = GetMesh();

            Vector2 size = ShapeMaker.Size(mesh.vertices);
            Width = size.x * Measurement.mm;
            Height = size.y * Measurement.mm;
            Mass = 0.0647989 * Measurement.g;
        }

        private Mesh GetMesh()
        {
            Mesh mesh = ShapeMaker.GetRegularShape(5, 1);
            mesh = ShapeMaker.SubdivideVerts(mesh, 2);
            mesh = ShapeMaker.RandomizeMesh(mesh, .3f, RandomNumberGenerator);
            mesh = ShapeMaker.CenterMesh(mesh);
            return mesh;
        }

        public enum Color
        {
            Yellow,
            White,
            Orange,
            Red,
            Black,
            Green,
            Cyan,
        }
    }
}
