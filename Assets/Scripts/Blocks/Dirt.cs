using UnityEngine;

namespace Universe
{
    public class Dirt : Block
    {
        public override string BlockName => "Dirt";

        public override string Path => "Blocks/Dirt";

        public override void BlockCreate(Vector2 pos) { }
    }
}
