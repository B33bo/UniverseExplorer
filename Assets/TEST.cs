using UnityEngine;

namespace Universe
{
    public class TEST : MonoBehaviour
    {
        [SerializeField]
        private int x;
        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = ShapeMaker.GetRegularShape(5, 1);
        }

        private void OnValidate()
        {
            GetComponent<MeshFilter>().mesh = ShapeMaker.GetRegularShape(x, 5f / x);
        }
    }
}
