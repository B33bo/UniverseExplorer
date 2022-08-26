using UnityEngine;
using System.Collections.Generic;
using Universe.CelestialBodies.Planets;

namespace Universe.CelestialBodies.Biomes
{
    public class Continent : CelestialBody
    {
        public const double MinMass = 4e21 * Measurement.Kg, MaxMass = 6e23 * Measurement.Kg;
        public override string TypeString => "Continent (" + continentType.ToString() + ")";

        public override string TravelTarget => travelTargets[continentType];
        public TerrestrialPlanet planet;

        public override bool Circular => false;

        private static readonly Dictionary<ContinentType, string> travelTargets = new Dictionary<ContinentType, string>()
        {
            {ContinentType.Grass, "GrassBiome" },
            {ContinentType.Snow, "SnowBiome"},
            {ContinentType.Sand, "SandBiome" },
        };

        public Mesh mesh;
        public ContinentType continentType;

        public override void Create(Vector2 pos)
        {
            Name = RandomNum.GetPlanetName(RandomNumberGenerator);
            Position = pos;
            GenerateMesh();
            continentType = (ContinentType)RandomNum.Get(0, 3, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        private void GenerateMesh()
        {
            Mesh continentMesh = ShapeMaker.GetRegularShape(10, 1);

            continentMesh = ShapeMaker.RandomizeMesh(continentMesh, .5f, RandomNumberGenerator);
            continentMesh.name = "Continent";

            Vector2 min = Vector3.zero, max = Vector3.zero;

            for (int i = 0; i < continentMesh.vertexCount; i++)
            {
                Vector3 vertex = continentMesh.vertices[i];

                if (vertex.x < min.x)
                    min.x = vertex.x;
                else if (vertex.x > max.x)
                    max.y = vertex.y;

                if (vertex.y < min.y)
                    min.y = vertex.y;
                else if (vertex.y > max.y)
                    max.y = vertex.y;
            }

            Vector3 scale = new Vector3(max.x - min.x, max.y - min.y);
            Width = scale.x * 1000;
            Height = scale.y * 1000;

            mesh = ShapeMaker.CenterMesh(continentMesh);
        }

        public enum ContinentType
        {
            Grass,
            Sand,
            Snow = 2,
        }

        public Color GetColor()
        {
            string filePath = continentType switch
            {
                ContinentType.Grass => "Materials/Grass",
                ContinentType.Sand => "Materials/Sand",
                ContinentType.Snow => "Materials/Snow",
                _ => "Materials/error",
            };
            return Resources.Load<Material>(filePath).color;
        }
    }
}
