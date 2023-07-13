using UnityEngine;

namespace Universe
{
    public class BlockBorder : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sp;

        private void Update()
        {
            if (Block.blockSelected is null)
            {
                sp.enabled = false;
                return;
            }

            sp.enabled = true;
            transform.position = Block.blockSelected.transform.position;
        }
    }
}
