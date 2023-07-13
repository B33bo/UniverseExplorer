using UnityEngine;

namespace Universe
{
    public class Air : Block
    {
        public override string BlockName => "Air";

        public override string Path => "Blocks/Air";

        public override void BlockCreate(Vector2 pos)
        {
            Destroy(gameObject);
        }
    }
}
