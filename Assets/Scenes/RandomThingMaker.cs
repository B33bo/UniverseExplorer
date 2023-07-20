using UnityEngine;
using UnityEngine.AI;

namespace Universe
{
    public class RandomThingMaker : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter randomThingMaker;
        private void Start()
        {
            int sides = Random.Range(3, 7);
            Mesh newMesh = ShapeMaker.GetRegularShape(sides, 1);
            Instantiate(randomThingMaker).mesh = newMesh;
            Branch(newMesh, GetBottom(newMesh), 1);
        }

        private void Branch(Mesh mesh, Vector2 oldPosition, int depth)
        {
            int sides = Random.Range(3, 7);
            Mesh newMesh = ShapeMaker.GetRegularShape(sides, 1);
            var newMeshFilter = Instantiate(randomThingMaker);

            newMeshFilter.mesh = newMesh;
            newMeshFilter.transform.position = oldPosition;
        }

        private Vector2 GetBottom(Mesh mesh)
        {
            Vector2 pos = mesh.vertices[mesh.vertices.Length - 1] + mesh.vertices[0];
            return pos / 2;
        }
    }
}
