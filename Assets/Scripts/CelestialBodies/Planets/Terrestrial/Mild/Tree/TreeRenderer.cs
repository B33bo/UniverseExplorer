using UnityEngine;

namespace Universe.CelestialBodies.Planets.Grass
{
    public class TreeRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private Transform[] lowLevelBranches;

        [SerializeField]
        private SpriteRenderer[] leaves;

        [SerializeField]
        private bool IsDead;

        [SerializeField]
        private bool GenerateType;

        private const int colorBiomeWidth = 100;
        private bool isRainbow = false;

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
            TreePlant tree = new();
            Target = tree;

            if (IsDead)
                tree.IsDead = true;

            if (GenerateType)
                tree.SetType(new System.Random(Mathf.FloorToInt(pos.x / colorBiomeWidth) * colorBiomeWidth));

            if (seed.HasValue)
                tree.SetSeed(seed.Value);

            tree.Create(pos);

            for (int i = 0; i < lowLevelBranches.Length; i++)
                ReloadBranch(lowLevelBranches[i], 1.25f);

            if (tree.colorType == TreePlant.TreeColor.Rainbow)
            {
                isRainbow = true;
                rainbowTreeColorValues = new Color[leaves.Length];
            }

            ReloadLeaves();
            Target.OnInspected += v => ReloadLeaves();
        }

        private void ReloadLeaves()
        {
            var tree = Target as TreePlant;
            tree.SetTreeColorToType(tree.colorType, tree.RandomNumberGenerator);

            for (int i = 0; i < leaves.Length; i++)
            {
                Color currentLeafColor = tree.color;

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
