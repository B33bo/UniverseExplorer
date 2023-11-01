using UnityEngine;
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

                if (ColorHighlights.Instance)
                {
                    Color color = Color.Lerp(ColorHighlights.Instance.primary, ColorHighlights.Instance.secondary, RandomNum.GetFloat(1, Target.RandomNumberGenerator));
                    meshFilters[i].GetComponent<MeshRenderer>().material.color = color;
                    rockPile.colors[i] = color;
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
