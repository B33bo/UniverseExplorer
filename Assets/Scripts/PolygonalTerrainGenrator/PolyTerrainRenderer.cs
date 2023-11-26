using UnityEngine;
using System.Collections.Generic;
using Universe.Animals;

namespace Universe.Terrain
{
    public class PolyTerrainRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private CelestialBodyRenderer[] Prefabs;

        [SerializeField]
        private float[] Weights;

        [SerializeField]
        private SpawnMethod spawnMethod;

        [SerializeField]
        private AnimalSpawner animalSpawner;

        [SerializeField]
        private bool stretchUVs;

        private float weightSum;

        private List<CelestialBodyRenderer> spawned = new List<CelestialBodyRenderer>();
        private List<AnimalRenderer> animalsSpawned = new List<AnimalRenderer>();

        enum SpawnMethod
        {
            Inside,
            InsideFast,
            Top,
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new PolyTerrain();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
        }

        public void Spawn(Vector2 pos, int? seed, PolyTerrain previous, PolyTerrainLayer layer)
        {
            PolyTerrain polyTerrain = new PolyTerrain();
            Target = polyTerrain;

            if (seed.HasValue)
                polyTerrain.SetSeed(seed.Value);

            polyTerrain.Create(pos, layer);
            polyTerrain.height = layer.MinimumHeight;

            if (previous == null)
            {
                meshFilter.mesh = CreateMesh(null, 0);
            }
            else
            {
                meshFilter.mesh = CreateMesh(previous.points, previous.height);
            }

            if (layer.OverrideColor)
                meshRenderer.material.color = layer.color;

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            weightSum = 0;
            for (int i = 0; i < Weights.Length; i++)
                weightSum += Weights[i];

            if (spawnMethod == SpawnMethod.Inside)
            {
                SpawnObjectsInside();
                SpawnAnimals();
            }
            else if (spawnMethod == SpawnMethod.InsideFast)
                SpawnObjectsInsideFast();
            else
                SpawnObjectsTop();
        }

        private int GetRandomObject(System.Random rand)
        {
            return RandomNum.GetIndexFromWeights(Weights, RandomNum.GetFloat(0, weightSum, rand));
        }

        private void SpawnAnimals()
        {
            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                SpawnAnimalAtPoint(x, Target as PolyTerrain);
            }
        }

        protected virtual AnimalSpawner GetWalkingAnimals() => AnimalSpawner.WalkingAnimals;
        protected virtual AnimalSpawner GetInsideAnimals() => AnimalSpawner.InsideAnimals;

        private void SpawnAnimalAtPoint(float x, PolyTerrain poly)
        {
            var walkingAnimals = GetWalkingAnimals();
            var insideAnimals = GetInsideAnimals();

            if (!walkingAnimals && !insideAnimals)
                return;

            float targetY = poly.HeightAt(x);
            Vector2 pos = new Vector2(x, targetY);
            pos += (Vector2)Target.Position;

            int seed = pos.HashPos(poly.ObjectSeed);

            if (walkingAnimals)
                SpawnAnimalAt(walkingAnimals, pos, seed);

            if (!insideAnimals)
                return;

            var rand = new System.Random(seed);
            RandomNum.Init(rand);
            float randomY = RandomNum.GetFloat(targetY, rand);

            pos.y = randomY + Target.Position.y;
            SpawnAnimalAt(insideAnimals, pos, seed);
        }

        private void SpawnAnimalAt(AnimalSpawner animal, Vector2 pos, int seed)
        {
            var newAnimal = animal.SpawnAnimalAt(pos, seed);

            if (newAnimal)
            {
                newAnimal.AutoDestroy = false;
                animalsSpawned.Add(newAnimal);
            }
        }

        private void SpawnObjectsInside()
        {
            PolyTerrain poly = Target as PolyTerrain;
            System.Random rand = new System.Random(poly.ObjectSeed);

            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                float targetY = poly.HeightAt(x);
                for (int y = 0; y < targetY; y++)
                {
                    int objectIndex = GetRandomObject(rand);

                    if (Prefabs[objectIndex] == null)
                        continue;

                    Vector2 position = new Vector2(x, y);

                    SpawnObject(position, objectIndex, rand.Next());
                }
            }
        }

        private void SpawnObjectsInsideFast()
        {
            PolyTerrain poly = Target as PolyTerrain;
            System.Random rand = new System.Random(poly.ObjectSeed);

            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                SpawnAnimalAtPoint(x, poly);
                float targetY = poly.HeightAt(x);
                int objectIndex = GetRandomObject(rand);

                if (Prefabs[objectIndex] == null)
                    continue;

                Vector2 position = new Vector2(x, RandomNum.GetFloat(0, targetY, rand));

                SpawnObject(position, objectIndex, rand.Next());
            }
        }

        private void SpawnObject(Vector2 position, int index, int seed)
        {
            var newObject = Instantiate(Prefabs[index], transform);
            newObject.Spawn(position, seed);
            spawned.Add(newObject);
        }

        private void SpawnObjectsTop()
        {
            PolyTerrain poly = Target as PolyTerrain;
            System.Random rand = new System.Random(poly.ObjectSeed);

            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                SpawnAnimalAtPoint(x, poly);

                int objectIndex = GetRandomObject(rand);
                float targetY = poly.HeightAt(x);

                if (Prefabs[objectIndex] == null)
                    continue;

                Vector2 position = new Vector2(x, targetY);

                SpawnObject(position, objectIndex, rand.Next());
            }
        }

        private Mesh CreateMesh(Vector2[] previousPoints, float bottomDecrease)
        {
            PolyTerrain self = Target as PolyTerrain;

            if (previousPoints == null)
            {
                previousPoints = new Vector2[self.points.Length];

                for (int i = 0; i < previousPoints.Length; i++)
                    previousPoints[i] = new Vector2(self.points[i].x, 0);
            }

            Vector3[] verts = new Vector3[self.points.Length + previousPoints.Length];
            Vector2[] uv = new Vector2[verts.Length];

            for (int i = 0; i < verts.Length; i++)
            {
                if (i < self.points.Length)
                {
                    verts[i] = self.points[i];

                    if (stretchUVs)
                        uv[i] = new Vector2(i / (float)(self.points.Length - 1), 1);
                    else
                        uv[i] = new Vector2(i / (float)(self.points.Length - 1), verts[i].y / PolyTerrain.RealWidth);
                    continue;
                }

                verts[i] = previousPoints[previousPoints.Length - (i - self.points.Length) - 1];

                uv[i] = new Vector2(1 - ((i % self.points.Length) / (self.points.Length - 1f)), 0);
                verts[i].y -= bottomDecrease;
            }

            int[] tris = new int[(verts.Length - 1) * 6];

            for (int i = 0; i < verts.Length - 1; i++)
            {
                int triStartIndex = i * 6;
                tris[triStartIndex] = i;
                tris[triStartIndex + 1] = verts.Length - i - 2;
                tris[triStartIndex + 2] = verts.Length - i - 1;

                tris[triStartIndex + 3] = i;
                tris[triStartIndex + 4] = i + 1;
                tris[triStartIndex + 5] = verts.Length - i - 2;
            }

            return new Mesh
            {
                vertices = verts,
                triangles = tris,
                uv = uv,
            };
        }

        protected override void Destroyed()
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i].IsDestroyed)
                    continue;
                Destroy(spawned[i].gameObject);
            }

            for (int i = 0; i < animalsSpawned.Count; i++)
            {
                if (animalsSpawned[i].IsDestroyed)
                    continue;
                Destroy(animalsSpawned[i].gameObject);
            }
        }
    }
}
