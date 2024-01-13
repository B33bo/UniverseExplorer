using System.Collections.Generic;
using UnityEngine;
using Universe;

public class TreeNodeMeshDrawer
{
    private const float NinetyDegrees = .5f * Mathf.PI;

    private static readonly List<Vector3> points = new();
    private static readonly List<Vector2> UVs = new();
    private static readonly List<int> tris = new();
    private static UVMode UV_Mode;
    private static int maxDepth;

    public enum UVMode
    {
        None,
        DepthBased,
        Alternating,
    }

    public static Mesh GenerateMesh(TreeNode tree, UVMode uvMode = UVMode.None)
    {
        points.Clear();
        tris.Clear();
        UVs.Clear();

        Vector2 leftTranslate = GetLeftTranslate(tree.rotation) * tree.width;

        points.Add(tree.startPoint + leftTranslate);
        points.Add(tree.startPoint - leftTranslate);

        UVs.Add(Vector2.zero);
        UVs.Add(new Vector2(1, 0));
        maxDepth = tree.depth;
        UV_Mode = uvMode;

        AddBranch(tree, 0);

        Mesh mesh = new()
        {
            vertices = points.ToArray(),
            triangles = tris.ToArray(),
        };

        if (uvMode != UVMode.None)
            mesh.uv = UVs.ToArray();
        return mesh;
    }

    private static void AddBranch(TreeNode branch, int left)
    {
        Vector3 leftTranslate = GetLeftTranslate(branch.rotation) * branch.width;
        int newLeft = points.Count;

        // left translate is first. left + 1 is right.
        points.Add((Vector3)branch.endPoint + leftTranslate);
        points.Add((Vector3)branch.endPoint - leftTranslate);

        if (UV_Mode == UVMode.Alternating)
        {
            int yValue = (branch.depth + 1) % 2; // one less if statement CPU PIPELINING MOMENT
            UVs.Add(new Vector2(0, yValue));
            UVs.Add(new Vector2(1, yValue));
        }
        else if (UV_Mode == UVMode.DepthBased)
        {
            float yValue = (maxDepth - branch.depth + 1) / (float)(maxDepth + 1);
            UVs.Add(new Vector2(0, yValue));
            UVs.Add(new Vector2(1, yValue));
        }

        tris.Add(left);
        tris.Add(newLeft);
        tris.Add(newLeft + 1);

        tris.Add(left);
        tris.Add(newLeft + 1);
        tris.Add(left + 1);

        if (branch.Branches == null)
            return;

        for (int i = 0; i < branch.Branches.Length; i++)
            AddBranch(branch.Branches[i], newLeft);
    }

    private static Vector3 GetLeftTranslate(float rotation) => new Vector2(Mathf.Cos(rotation + NinetyDegrees), Mathf.Sin(rotation + NinetyDegrees));
}
