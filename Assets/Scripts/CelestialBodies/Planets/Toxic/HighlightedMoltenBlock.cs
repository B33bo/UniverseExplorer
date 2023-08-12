using UnityEngine;
using Universe.Blocks;

namespace Universe
{
    public class HighlightedMoltenBlock : DarkstoneRenderer
    {
        [SerializeField]
        private float alpha = 1;

        public override void Spawn(Vector2 pos, int? seed)
        {
            base.Spawn(pos, seed);

            Color start = ColorHighlights.Instance.primary;
            gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(start, 0)}, new GradientAlphaKey[] {
            new GradientAlphaKey(alpha, 0), new GradientAlphaKey(0, .5f), new GradientAlphaKey(alpha, 1)});
        }
    }
}
