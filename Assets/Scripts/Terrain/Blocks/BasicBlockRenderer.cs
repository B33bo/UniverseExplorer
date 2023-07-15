using UnityEngine;

namespace Universe.Blocks
{
    public class BasicBlockRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private string blockName, travelTarget;

        public override void Spawn(Vector2 pos, int? seed)
        {
            var basicBlock = new BasicBlock();
            Target = basicBlock;

            if (seed.HasValue)
                basicBlock.SetSeed(seed.Value);

            basicBlock.Create(pos, blockName, travelTarget);
        }
    }
}
