using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Universe
{
    public static class ShapeMaker
    {
        public static Mesh GetRegularShape(int points, float radius)
        {
            Vector3[] verts = new Vector3[points];
            Vector2[] UVs = new Vector2[points];
            int[] tris = new int[(points - 2) * 3];

            for (int i = 0; i < points; i++)
            {
                float theta = Mathf.PI * -2 * i / points;
                Vector2 position = new(Mathf.Cos(theta), Mathf.Sin(theta));

                UVs[i] = (position + Vector2.one) * .5f;

                position *= radius;
                verts[i] = position;

                if (i >= 2)
                {
                    int triIndex = (i - 2) * 3;
                    tris[triIndex] = 0;
                    tris[triIndex + 1] = i - 1;
                    tris[triIndex + 2] = i;
                }
            }

            return new Mesh()
            {
                vertices = verts,
                triangles = tris,
                uv = UVs,
                name = points + "-gon",
            };
        }

        public static Mesh GetStar(int points, float lengthOfLines, float pointiness)
        {
            Vector3[] verts = new Vector3[2 * points];
            Vector2[] UVs = new Vector2[2 * points];
            int[] tris = new int[(2 * points - 2) * 3];

            float correction = Mathf.PI * 2f / verts.Length + (Mathf.PI * .5f);
            for (int i = 0; i < verts.Length; i++)
            {
                float theta = Mathf.PI * 2 * -i / verts.Length + correction;
                Vector2 position = new(Mathf.Cos(theta), Mathf.Sin(theta));

                UVs[i] = (position + Vector2.one) * .5f;

                position *= lengthOfLines;
                bool spike = i % 2 == 1;

                if (spike)
                    position *= pointiness;

                verts[i] = position;

                if (i < points)
                {
                    int triIndex = i * 3;
                    tris[triIndex] = 2 * i;
                    tris[triIndex + 1] = (2 * i + 1) % verts.Length;
                    tris[triIndex + 2] = (2 * i + 2) % verts.Length;
                }
                else if (i < verts.Length - 2)
                {
                    int triIndex = i * 3;
                    tris[triIndex] = 0;
                    tris[triIndex + 1] = (i - points + 1) * 2;
                    tris[triIndex + 2] = (i - points + 1) * 2 + 2;
                }
            }

            return new Mesh()
            {
                vertices = verts,
                triangles = tris,
                uv = UVs,
            };
        }

        public static Mesh SubdividedRectangle(Vector2 scale, int subdivisions)
        {
            Vector3[] verts = new Vector3[subdivisions * 2 + 4];
            Vector2[] UVs = new Vector2[verts.Length];
            int[] tris = new int[3 * (verts.Length - 2)];
            int trisEndPoint = (verts.Length - 1) / 2;

            UVs[0] = Vector2.zero;
            UVs[1] = new(0, 1);
            UVs[subdivisions + 2] = new(1, 1);
            UVs[subdivisions + 3] = new(1, 0);

            for (int i = 0; i < verts.Length; i++)
            {
                if (i < subdivisions)
                {
                    float x = (i + 1f) / (subdivisions + 1f);
                    UVs[i + 2] = new(x, 1);
                    UVs[UVs.Length - i - 1] = new(x, 0);
                }

                verts[i] = (UVs[i] - new Vector2(.5f, .5f)) * scale;

                if (i >= trisEndPoint)
                    continue;

                tris[6 * i] = i;
                tris[6 * i + 1] = i + 1;
                tris[6 * i + 2] = verts.Length - i - 1;

                tris[6 * i + 3] = verts.Length - i - 1;
                tris[6 * i + 4] = i + 1;
                tris[6 * i + 5] = verts.Length - i - 2;
            }

            Mesh mesh = new()
            {
                name = "Cloud",
                vertices = verts,
                uv = UVs,
                triangles = tris,
            };

            return mesh;
        }

        public static Mesh RandomizeMesh(Mesh mesh, float randomAmount, System.Random random)
        {
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector2 newVec = GetVector(randomAmount, mesh.vertices[i], random);
                vertices[i] = newVec;
                continue;
            }
            mesh.vertices = vertices;
            return mesh;
        }

        private static Vector2 GetVector(float randomAmount, Vector2 baseVector, System.Random random)
        {
            return new Vector2(
                baseVector.x + (float)RandomNum.Get(-randomAmount, randomAmount, random),
                baseVector.y + (float)RandomNum.Get(-randomAmount, randomAmount, random)
            );
        }

        private static Mesh CenterMesh(Mesh mesh, out Vector3 bottomLeft, out Vector3 topRight)
        {
            bottomLeft = Vector2.zero;
            topRight = Vector2.zero;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                if (mesh.vertices[i].x < bottomLeft.x)
                    bottomLeft.x = mesh.vertices[i].x;
                else if (mesh.vertices[i].y > topRight.x)
                    topRight.x = mesh.vertices[i].x;

                if (mesh.vertices[i].y < bottomLeft.y)
                    bottomLeft.y = mesh.vertices[i].y;
                else if (mesh.vertices[i].y > topRight.y)
                    topRight.y = mesh.vertices[i].y;
            }

            Vector3 difference = topRight - bottomLeft;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                mesh.vertices[i] += difference;
            }

            bottomLeft += difference;
            topRight += difference;
            return mesh;
        }

        public static Mesh CenterMesh(Mesh mesh)
        {
            return CenterMesh(mesh, out _, out _);
        }

        private static Mesh RemoveUnusedVerticies(Mesh mesh)
        {
            List<Vector3> verticies = mesh.vertices.ToList();
            List<int> tris = mesh.triangles.ToList();

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                if (tris.Contains(i))
                    continue;
                verticies.RemoveAt(i);
                for (int j = 0; j < tris.Count; j++)
                {
                    if (tris[j] > i)
                        tris[j]--;
                }
                i--;
            }

            mesh.vertices = verticies.ToArray();
            mesh.triangles = tris.ToArray();
            return mesh;
        }

        public static Mesh NormalizeMesh(Mesh mesh, out Vector3 bottomLeft, out Vector3 topRight)
        {
            return CenterMesh(RemoveUnusedVerticies(mesh), out bottomLeft, out topRight);
        }

        public static Mesh SubdivideVerts(Mesh mesh, int subdivisions)
        {
            Vector3[] verts = mesh.vertices;
            Vector3[] newVerts = new Vector3[mesh.vertexCount * (subdivisions + 1)];

            float lerpPerSubdivision = 1f / (subdivisions + 1);

            int realIndex = 0;
            for (int i = 0; i < newVerts.Length; i += subdivisions + 1)
            {
                newVerts[i] = verts[realIndex];
                realIndex++;
            }

            int previous = -1;
            int next = 0;
            float currentLerpVal = 0;

            for (int i = 0; i < newVerts.Length; i++)
            {
                if (i == next)
                {
                    previous = next;

                    int nextIndex = i + subdivisions + 1;
                    if (nextIndex >= newVerts.Length)
                        nextIndex = 0;

                    next = nextIndex;
                    currentLerpVal = 0;
                    continue;
                }

                currentLerpVal += lerpPerSubdivision;
                newVerts[i] = Vector3.Lerp(newVerts[previous], newVerts[next], currentLerpVal);
            }

            int[] tris = new int[newVerts.Length * 3];
            for (int i = 1; i < newVerts.Length; i++)
            {
                int triIndex = i * 3;
                tris[triIndex] = 0;
                tris[triIndex + 1] = i - 1;
                tris[triIndex + 2] = i;
            }

            mesh.vertices = newVerts;
            mesh.triangles = tris;

            return mesh;
        }

        public static Vector2 Size(Vector3[] mesh)
        {
            Vector2 bottomLeft = Vector2.zero, topRight = Vector2.zero;

            for (int i = 0; i < mesh.Length; i++)
            {
                if (mesh[i].x < bottomLeft.x)
                    bottomLeft.x = mesh[i].x;
                else if (mesh[i].x > topRight.x)
                    topRight.x = mesh[i].x;

                if (mesh[i].y > topRight.y)
                    topRight.y = mesh[i].y;
                else if (mesh[i].y < bottomLeft.y)
                    bottomLeft.y = mesh[i].y;
            }

            return topRight - bottomLeft;
        }

        public static Vector2[] ToVector2(this Vector3[] v)
        {
            Vector2[] result = new Vector2[v.Length];
            for (int i = 0; i < v.Length; i++)
                result[i] = v[i];
            return result;
        }

        public static TreeNode GetRandomWebLines(System.Random rand)
        {
            WebTreeNode root = new();
            root.StartTree(Vector2.zero, 2, rand);
            return root;
        }

        private class WebTreeNode : TreeNode
        {
            protected override float GetRotation()
            {
                return RandomNum.GetFloat(0, Mathf.PI * 2, random);
            }

            protected override float GetLength()
            {
                return float.NaN;
            }

            protected override int GetBranchCount()
            {
                if (Parent == null)
                    return 4;
                return 2;
            }

            protected override Vector2 GetEndPoint(Vector2 movement)
            {
                Vector2 endPoint = new(Mathf.Cos(rotation), Mathf.Sin(rotation));
                length = float.NaN;
                return endPoint;
            }
        }
    }
}
