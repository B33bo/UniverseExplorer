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
        private int[] highLightIndex;

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

                Color color = Highlight(i) ? new(0, t, 1) : new(t, t, t);

                Gizmos.color = color;

                Vector3 vertPoint = verts[i];
                vertPoint.x *= scale.x;
                vertPoint.y *= scale.y;
                vertPoint.z *= scale.z;

                Gizmos.DrawSphere(transform.position + vertPoint, radius);
            }

            if (gizmoTris)
                Tris(mesh);
        }

        private void Tris(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;
            int[] tris = mesh.triangles;

            for (int i = 0; i < tris.Length; i += 3)
            {
                float t = (i * 3f) / (tris.Length - 1);
                Gizmos.color = Color.HSVToRGB(t, .33f, 1);

                Gizmos.DrawLine(verts[tris[i]] + transform.position, verts[tris[i + 1]] + transform.position);

                Gizmos.color = Color.HSVToRGB(t, .66f, 1);

                Gizmos.DrawLine(verts[tris[i + 1]] + transform.position, verts[tris[i + 2]] + transform.position);

                Gizmos.color = Color.HSVToRGB(t, 1, 1);

                Gizmos.DrawLine(verts[tris[i + 2]] + transform.position, verts[tris[i]] + transform.position);
            }
        }

        private bool Highlight(int index)
        {
            if (highLightIndex == null)
                return false;
            for (int i = 0; i < highLightIndex.Length; i++)
            {
                if (highLightIndex[i] == index)
                    return true;
            }
            return false;
        }
    }
}
#endif