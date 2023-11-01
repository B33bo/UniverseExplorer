using UnityEngine;

namespace Universe.CelestialBodies.Planets.Water
{
    public class SandGrain : CelestialBody
    {
        public override string TypeString => "Sand Grain";

        public override string TravelTarget => "ChemicalComposition";

        public override bool Circular => false;

        public Mesh mesh;
        public Color color;

        public static float[] ColorWeightChars = new float[]
        {
            71, //yellow
            10, //white
            10, //orange
            5,  //black
            1,  //red
            1,  //green
            1,  //cyan
            1,  //random color
        };
        private const float SumOfWeightCharts = 100;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            Name = "Sand Grain";
            mesh = GetMesh();

            Vector2 size = ShapeMaker.Size(mesh.vertices);
            Width = size.x * Measurement.mm;
            Height = size.y * Measurement.mm;
            Mass = 0.0647989 * Measurement.g * ((size.x + size.y) / 2);

            color = (Color)RandomNum.GetIndexFromWeights(ColorWeightChars,
                RandomNum.GetFloat(0, SumOfWeightCharts, RandomNumberGenerator));

            composition = new ChemicalComposition(ChemicalComposition.BondingType.Covalent, new Chemical(100, "SiO2"));
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
            Random,
        }

        public override string GetBonusTypes()
        {
            return $"Color - {color}";
        }
    }
}
