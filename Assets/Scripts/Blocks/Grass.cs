using UnityEngine;

namespace Universe.Blocks
{
    public class Grass : Block
    {
        public override string BlockName => "Grass";

        public override string Path => "Blocks/Grass";

        public override void BlockCreate(Vector2 pos) { }
    }
}
