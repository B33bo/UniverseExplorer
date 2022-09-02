using UnityEngine;
using Universe.CelestialBodies;
using Universe.CelestialBodies.Planets;
using Universe.CelestialBodies.Planets.Rocky;

namespace Universe
{
    public class RockPileRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter[] meshFilters;

        public override void Spawn(Vector2 pos, int? seed)
        {
            RockPile rockPile = new RockPile();
            Target = rockPile;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            float highestPos = 0;
            MeshFilter lastMeshFilter = null;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (rockPile.Rocks <= i)
                {
                    Destroy(meshFilters[i].gameObject);
                    continue;
                }

                meshFilters[i].mesh = rockPile[i];

                highestPos = HighestPointOf(rockPile[i].vertices).y + meshFilters[i].transform.position.y + .1f;

                if (FindObjectOfType<TerrainGenerator>())
                {
                    Color color;
                    if (BodyManager.Parent is RockyPlanet rockyPlanet)
                        color = rockyPlanet.RockColor;
                    else
                        color = TerrainGenerator.Instance.BiomeAtPosition(Target.Position.x).groundColor;

                    ColorHSV colorHSV = color;

                    colorHSV.s += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);
                    colorHSV.v += RandomNum.GetFloat(-.1f, .1f, Target.RandomNumberGenerator);

                    Color newColor = colorHSV;
                    meshFilters[i].GetComponent<MeshRenderer>().material.color = newColor;
                    rockPile.colors[i] = newColor;
                }
                else
                    rockPile.colors[i] = meshFilters[i].GetComponent<MeshRenderer>().material.color;

                lastMeshFilter = meshFilters[i];
                if (i + 1 >= meshFilters.Length)
                    continue;
                meshFilters[i + 1].transform.position = new Vector3(transform.position.x, highestPos);
            }

            cameraLerpTarget.transform.localPosition = new Vector3(0, lastMeshFilter.transform.localPosition.y / 2);
        }

        private Vector3 HighestPointOf(Vector3[] verticies)
        {
            Vector2 currentMax = verticies[0];
            for (int i = 1; i < verticies.Length; i++)
            {
                if (currentMax.y < verticies[i].y)
                    currentMax = verticies[i];
            }
            return currentMax;
        }
    }
}
