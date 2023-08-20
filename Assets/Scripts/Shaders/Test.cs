using UnityEngine;

namespace Universe
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private Color red, green, blue;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer.material.SetColor("_ColorA", red);
            spriteRenderer.material.SetColor("_ColorB", green);
            spriteRenderer.material.SetColor("_ColorC", blue);
        }
    }
}
