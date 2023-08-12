using UnityEngine;
using Universe.CelestialBodies.Biomes.Grass;

namespace Universe
{
    public class TreeRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform[] lowLevelBranches;

        [SerializeField]
        private SpriteRenderer[] leaves;

        [SerializeField]
        private bool IsDead;

        private bool isRainbow = false;

        private static readonly Color[] ColorValuesForLeaves = new Color[]
        {
            new Color(0, .8f, 0), //Green
            new Color(0, 0, 0, 0), //Dead / transparent
            new Color(1, 0, 0), //Red
            new Color(1, .5f, 0), //Orange
            new Color(1, .5f, 1), // Pink
            new Color(1, 1, 1), // Rainbow
        };

        private Color[] rainbowTreeColorValues;

        private void ReloadBranch(Transform branch, float maxYpos)
        {
            if (branch.parent == transform)
                return;

            float newYPos = RandomNum.GetFloat(maxYpos, Target.RandomNumberGenerator);
            branch.localPosition = new Vector3(0, newYPos);

            ReloadBranch(branch.parent, maxYpos * 2);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            TreePlant tree = new TreePlant();
            Target = tree;

            if (IsDead)
                tree.IsDead = true;

            if (seed.HasValue)
                tree.SetSeed(seed.Value);

            tree.Create(pos);

            for (int i = 0; i < lowLevelBranches.Length; i++)
                ReloadBranch(lowLevelBranches[i], 1.25f);

            if (tree.color == TreePlant.TreeColor.Rainbow)
            {
                isRainbow = true;
                rainbowTreeColorValues = new Color[leaves.Length];
            }

            ReloadLeaves();
            Target.OnInspected += v => { if (v.VariableName == "Type") ReloadLeaves(); };
        }

        private void ReloadLeaves()
        {
            var tree = Target as TreePlant;
            for (int i = 0; i < leaves.Length; i++)
            {
                Color currentLeafColor = ColorValuesForLeaves[(int)tree.color];

                currentLeafColor.r += RandomNum.GetFloat(-.2f, .2f, Target.RandomNumberGenerator);
                currentLeafColor.g += RandomNum.GetFloat(-.2f, .2f, Target.RandomNumberGenerator);
                currentLeafColor.b += RandomNum.GetFloat(-.2f, .2f, Target.RandomNumberGenerator);

                leaves[i].color = currentLeafColor;

                if (isRainbow)
                    rainbowTreeColorValues[i] = currentLeafColor;
            }
        }

        public override void OnUpdate()
        {
            if (!isRainbow)
                return;

            Color normalColor = Color.HSVToRGB((GlobalTime.Time / 10f) % 1, 1, 1);
            for (int i = 0; i < leaves.Length; i++)
            {
                leaves[i].color = new Color
                {
                    r = rainbowTreeColorValues[i].r * normalColor.r,
                    g = rainbowTreeColorValues[i].g * normalColor.g,
                    b = rainbowTreeColorValues[i].b * normalColor.b,
                    a = 1,
                };
            }
        }
    }
}
