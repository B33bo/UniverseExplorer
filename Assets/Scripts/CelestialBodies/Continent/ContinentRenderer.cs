using UnityEngine;
using UnityEngine.SceneManagement;
using Universe.CelestialBodies.Biomes;

namespace Universe.CelestialBodies
{
    public class ContinentRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshRenderer;

        [SerializeField]
        private PolygonCollider2D polygonCollider;

        [SerializeField]
        private Material groundColor, sandColor, snowColor;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Continent();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            Init();
        }

        public void Init()
        {
            Mesh mesh = (Target as Continent).mesh;
            meshRenderer.mesh = mesh;

            Continent.ContinentType continentType = (Target as Continent).continentType;
            GetComponent<MeshRenderer>().material = continentType switch
            {
                Continent.ContinentType.Grass => groundColor,
                Continent.ContinentType.Sand => sandColor,
                Continent.ContinentType.Snow => snowColor,
                _ => throw new System.NotImplementedException(),
            };

            polygonCollider.points = mesh.vertices.ToVector2();
            polygonCollider.enabled = SceneManager.GetActiveScene().name == "TerrestrialPlanet";
        }
    }
}
