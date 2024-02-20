using System.Collections.Generic;
using UnityEngine;
using Universe.Animals;

namespace Universe.Terrain
{
    public class PolyTerrainRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        protected MeshFilter meshFilter;

        [SerializeField]
        protected MeshRenderer meshRenderer;

        [SerializeField]
        private CelestialBodyRenderer[] InsidePrefabs;

        [SerializeField]
        private float[] InsideWeights;

        [SerializeField]
        private CelestialBodyRenderer[] SurfacePrefabs;

        [SerializeField]
        private float[] SurfaceWeights;

        [SerializeField]
        private bool insideFast = true;

        [SerializeField]
        private bool stretchUVs;

        [SerializeField]
        [Range(0, 2)] 
        private float animalSpawnChance;

        [SerializeField]
        private AnimalSpawnerType animalSpawnerType;

        [SerializeField]
        private EdgeCollider2D collision;

        private PolyTerrain previous;
        private PolyTerrainLayer layer;

        private readonly List<CelestialBodyRenderer> spawned = new();

        private void SpawnAnimalAtPoint(float x, float y)
        {
            AnimalSpawner.GetSpawner(animalSpawnerType).Spawn(x, y);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new PolyTerrain();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);
        }

        public virtual void Spawn(Vector2 pos, int? seed, PolyTerrain previous, PolyTerrainLayer layer)
        {
            PolyTerrain polyTerrain = new();
            Target = polyTerrain;

            if (seed.HasValue)
                polyTerrain.SetSeed(seed.Value);

            polyTerrain.Create(pos, layer);
            polyTerrain.height = layer.MinimumHeight;

            this.previous = previous;
            this.layer = layer;

            Regenerate();

            if (layer.OverrideColor)
                meshRenderer.material.color = layer.color;
            if (layer.UseColorHighlights)
                meshRenderer.material.color *= ColorHighlights.Instance.primary;

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            if (insideFast)
                SpawnObjectsInsideFast();
            else
                SpawnObjectsInside();
        }

        private int GetRandomObject(System.Random rand, float[] weights)
        {
            return RandomNum.GetIndexFromWeights(weights, rand);
        }

        private void SpawnObjectsInside()
        {
            PolyTerrain poly = Target as PolyTerrain;
            System.Random rand = new(poly.ObjectSeed);

            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                float targetY = poly.HeightAt(x);
                for (int y = 0; y < targetY; y++)
                {
                    if (InsideWeights.Length > 0)
                    {
                        int objectIndex = GetRandomObject(rand, InsideWeights);
                        SpawnObject(new Vector2(x, y), InsidePrefabs[objectIndex], rand.Next());
                    }
                }

                if (SurfaceWeights.Length > 0)
                {
                    int objectIndex = GetRandomObject(rand, SurfaceWeights);
                    SpawnObject(new Vector2(x, targetY), SurfacePrefabs[objectIndex], rand.Next());
                }
            }
        }

        private void SpawnObjectsInsideFast()
        {
            PolyTerrain poly = Target as PolyTerrain;
            System.Random rand = new(poly.ObjectSeed);

            for (int x = 0; x < PolyTerrain.RealWidth; x++)
            {
                float targetY = poly.HeightAt(x);

                // spawn inside objects
                if (InsideWeights.Length > 0)
                {
                    int objectIndex = GetRandomObject(rand, InsideWeights);
                    Vector2 position = new(x, RandomNum.GetFloat(0, targetY, rand));
                    SpawnObject(position, InsidePrefabs[objectIndex], rand.Next());
                }

                // spawn surface objects
                if (SurfaceWeights.Length > 0)
                {
                    int objectIndex = GetRandomObject(rand, SurfaceWeights);
                    Vector2 position = new(x, targetY);
                    SpawnObject(position, SurfacePrefabs[objectIndex], rand.Next());
                }

                if (Random.value < animalSpawnChance / PolyTerrain.RealWidth)
                    SpawnAnimalAtPoint(x + Target.Position.x, targetY + Target.Position.y);
            }
        }

        private void SpawnObject(Vector2 position, CelestialBodyRenderer prefab, int seed)
        {
            if (prefab == null)
                return;
            position += (Vector2)Target.Position;
            var newObject = Instantiate(prefab);
            newObject.Spawn(position, seed);
            spawned.Add(newObject);
        }

        public void ForcePointStart(int index, Vector2 value)
        {
            (Target as PolyTerrain).points[index] = value;
        }

        public void ForcePointEnd(int index, Vector2 value)
        {
            index++;
            (Target as PolyTerrain).points[^index] = value;
        }

        public void Regenerate()
        {
            if (previous == null)
                meshFilter.mesh = CreateMesh(null, 0);
            else
                meshFilter.mesh = CreateMesh(previous.points, previous.height);

            collision.points = (Target as PolyTerrain).points;
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
        }
    }
}
