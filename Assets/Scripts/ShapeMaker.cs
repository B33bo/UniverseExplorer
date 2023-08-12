using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Universe
{
    public static class ShapeMaker
    {
        public static Mesh GetRegularShape(int points, float lengthOfLines)
        {
            float AnglePerPoint = 360f / points;

            Vector3 position = Vector3.zero;
            float currentAngle = 0;

            Vector3[] verts = new Vector3[points];
            for (int i = 1; i < points; i++)
            {
                currentAngle += AnglePerPoint;
                float currentAngleRadians = currentAngle * Mathf.Deg2Rad;
                position += new Vector3(Mathf.Cos(currentAngleRadians), Mathf.Sin(currentAngleRadians)) * lengthOfLines;
                verts[i] = position;
            }

            int[] tris = new int[points * 3];
            Vector3 bottomLeft = verts[0], topRight = verts[0];

            for (int i = 1; i < verts.Length; i++)
            {
                if (verts[i].x < bottomLeft.x)
                    bottomLeft.x = verts[i].x;
                else if (verts[i].x > topRight.x)
                    topRight.x = verts[i].x;

                if (verts[i].y < bottomLeft.y)
                    bottomLeft.y = verts[i].y;
                else if (verts[i].y > topRight.y)
                    topRight.y = verts[i].y;

                if (i == 1)
                    continue;
                int triIndex = i * 3;
                tris[triIndex] = 0;
                tris[triIndex + 1] = i - 1;
                tris[triIndex + 2] = i;
            }

            for (int i = 0; i < verts.Length; i++)
                verts[i] -= (topRight - bottomLeft) / 2 + bottomLeft;

            Mesh mesh = new Mesh
            {
                name = $"regular {points} sided shape",
                vertices = verts,
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
            mesh.triangles = tris;//GetRegularShape(newVerts.Length, 1).triangles;

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

        public static List<(Vector2, Vector2)> GetRandomWebLines(System.Random rand)
        {
            // Start from the middle and draw 4 lines from the center to random points
            // from each of those lines, draw 2 lines originating from any point on the line and ending at the middle

            const int BaseLines = 4;
            List<(Vector2, Vector2)> lines = new List<(Vector2, Vector2)>();

            for (int i = 0; i < BaseLines; i++)
            {
                float rotation = i * (360f / BaseLines) + RandomNum.GetFloat(5, rand);

                //This was a bug, Mathf.Rad2Deg should be Mathf.Deg2Rad but doing that makes it look too normal, I prefer the chaotic sprawl of the bug
                float rotationRad = rotation * Mathf.Rad2Deg;

                Vector2 end = new Vector2(Mathf.Cos(rotationRad), Mathf.Sin(rotationRad)) / 2;
                lines.Add((Vector2.zero, end));

                lines.Add(GetBranch(end, rotation + 15));
                lines.Add(GetBranch(end, rotation - 15));
            }

            return lines;

            (Vector2, Vector2) GetBranch(Vector2 parentBranchEnd, float rotation)
            {
                float DistanceFromOrigin = RandomNum.GetFloat(1, rand);
                Vector2 startPos = Vector2.Lerp(Vector2.zero, parentBranchEnd, DistanceFromOrigin);

                Vector2 direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
                Vector2 endPos = startPos + direction;

                endPos = endPos.normalized / 2;
                return (startPos, endPos);
            }
        }
    }
}
