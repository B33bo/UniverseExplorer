using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets.Molten
{
    public class LavaFork : CelestialBody
    {
        public override string TypeString => "Lava";

        public override string TravelTarget => "";

        public override bool Circular => false;

        public const int MaxDepth = 4;
        public Node tree;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            tree = new Node();

            tree.StartTree(Vector2.zero, MaxDepth, RandomNumberGenerator);
        }

        public class Node : TreeNode
        {
            protected override float GetLength()
            {
                if (Parent == null)
                    return RandomNum.GetFloat(12, random);
                return RandomNum.GetFloat(8, random);
            }

            protected override int GetBranchCount()
            {
                return 2;
            }

            protected override float GetRotation()
            {
                if (Parent == null)
                    return Mathf.PI * 1.5f;

                float radius = 60 * Mathf.Deg2Rad;

                return Parent.rotation + RandomNum.GetFloat(-radius, radius, random);
            }

            protected override float GetWidth()
            {
                return .2f * ((depth + 1) / (MaxDepth + 1f));
            }

            public (float, float) GetMaxXMinY()
            {
                float maxX = Mathf.Abs(endPoint.x);
                float minY = endPoint.y;

                for (int i = 0; i < Branches.Length; i++)
                {
                    (float currentMaxX, float currentMinY) = (Branches[i] as Node).GetMaxXMinY();
                    if (currentMaxX > maxX)
                        maxX = currentMaxX;
                    if (currentMinY < minY)
                        minY = currentMinY;
                }
                return (maxX, minY);
            }
        }
    }
}
