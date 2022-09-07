using UnityEngine;

namespace Universe
{
    public class Stone : Block
    {
        public override string BlockName => "Stone";

        public override string Path => "Blocks/Stone";

        public override void BlockCreate(Vector2 pos) { }
    }
}
