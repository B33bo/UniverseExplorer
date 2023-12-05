using System;
using UnityEngine;

namespace Universe.CelestialBodies.Planets.Molten
{
    public class LavaFork : CelestialBody
    {
        public override string TypeString => "Lava";

        public override string TravelTarget => "";

        public override bool Circular => false;

        public Node tree;
        public int MaxDepth = 5;
        public float initOffset = 270;
        public float angle = 60;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            tree = new Node()
            {
                angleOffset = RandomNum.GetFloat(-angle, angle, RandomNumberGenerator) + initOffset,
                length = RandomNum.GetFloat(5, RandomNumberGenerator),
                depth = 0,
                branches = new Node[2],
            };

            SpawnNodes(tree);
        }

        private void SpawnNodes(Node node)
        {
            if (node.depth >= MaxDepth)
                return;

            for (int i = 0; i < node.branches.Length; i++)
            {
                Node newNode = new()
                {
                    angleOffset = RandomNum.GetFloat(-angle, angle, RandomNumberGenerator) + initOffset,
                    length = RandomNum.GetFloat(10, RandomNumberGenerator),
                    depth = node.depth + 1,
                    branches = new Node[2],
                };

                if (node.depth + 1 >= MaxDepth)
                    newNode.branches = Array.Empty<Node>();

                node.branches[i] = newNode;
                SpawnNodes(newNode);
            }
        }

        public class Node
        {
            public Node[] branches;
            public float angleOffset;
            public float length;
            public int depth;
        }
    }
}
