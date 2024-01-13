#if UNITY_EDITOR
using UnityEngine;

namespace Universe
{
    public class MeshChecker : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [SerializeField]
        [Range(.01f, .5f)]
        private float radius;

        [SerializeField]
        private int[] highlightIndex;

        [SerializeField]
        private int[] highlightTri;

        [SerializeField]
        private bool gizmoTris;

        private void OnDrawGizmosSelected()
        {
            if (meshFilter == null)
                return;

            Mesh mesh = meshFilter.sharedMesh;

            if (mesh == null)
                return;

            Vector3[] verts = mesh.vertices;

            Vector3 scale = transform.lossyScale;

            for (int i = 0; i < verts.Length; i++)
            {
                float t = i / (verts.Length - 1f);
                bool highlight = Highlight(i);

                Color color;

                if (highlight)
                {
                    if (HighlightTri(i))
                        color = new Color(1, 1, 0);
                    else
                        color = new(0, t, 1);
                }
                else if (HighlightTri(i))
                    color = new(0, 1, 0);
                else
                    color = new(t, t, t);

                Gizmos.color = color;

                Vector3 vertPoint = verts[i];
                vertPoint.x *= scale.x;
                vertPoint.y *= scale.y;
                vertPoint.z *= scale.z;

                Gizmos.DrawSphere(transform.position + vertPoint, Highlight(i) ? radius * 1.25f : radius);
            }
        }

        private bool Highlight(int index)
        {
            if (highlightIndex == null)
                return false;
            for (int i = 0; i < highlightIndex.Length; i++)
            {
                if (highlightIndex[i] == index)
                    return true;
            }
            return false;
        }

        private bool HighlightTri(int index)
        {
            if (highlightTri == null)
                return false;

            for (int i = 0; i < highlightTri.Length; i++)
            {
                int[] triangles = meshFilter.sharedMesh.triangles;

                int index1 = triangles[highlightTri[i] * 3];
                int index2 = triangles[highlightTri[i] * 3 + 1];
                int index3 = triangles[highlightTri[i] * 3 + 2];

                if (index == index1 || index == index2 || index == index3)
                    return true;
            }
            return false;
        }
    }
}
#endif