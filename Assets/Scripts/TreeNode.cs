using System;
using UnityEngine;

namespace Universe
{
    public partial class TreeNode
    {
        public Vector2 startPoint;
        public Vector2 endPoint;
        public float rotation;
        public float length;
        public float width;
        public System.Random random;

        public int depth;

        public TreeNode[] Branches;
        public TreeNode Parent;
        public int Index;

        public virtual void StartTree(Vector2 startPoint, int depth, System.Random random = null)
        {
            this.startPoint = startPoint;
            this.depth = depth;
            this.random = random;

            SpawnBranches();
        }

        public virtual void SpawnBranches()
        {
            if (Parent != null)
            {
                startPoint = Parent.endPoint;
                depth = Parent.depth - 1;
                random = GetRandom();
            }

            length = GetLength();
            rotation = GetRotation();
            width = GetWidth();

            Vector2 movement = length * new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
            endPoint = GetEndPoint(movement);

            if (depth <= 0)
            {
                Branches = Array.Empty<TreeNode>();
                return;
            }

            Branches = new TreeNode[GetBranchCount()];
            for (int i = 0; i < Branches.Length; i++)
            {
                Branches[i] = GetBranch();
                Branches[i].Parent = this;
                Branches[i].startPoint = endPoint;
                Branches[i].Index = i;
                Branches[i].SpawnBranches();
            }
        }

        protected virtual Vector2 GetEndPoint(Vector2 movement) => startPoint + movement;
        protected virtual float GetLength() => Parent == null ? 1 : Parent.length;
        protected virtual float GetRotation() => Parent == null ? Mathf.PI * .25f : Parent.rotation + UnityEngine.Random.Range(-.5f * Mathf.PI, .5f * Mathf.PI);
        protected virtual float GetWidth() => Parent == null ? .05f : Parent.width;
        protected virtual int GetBranchCount() => Parent == null ? 2 : Parent.Branches.Length;

        protected virtual TreeNode GetBranch()
        {
            TreeNode branch = (TreeNode)Activator.CreateInstance(GetType());
            return branch;
        }

        protected virtual System.Random GetRandom() => Parent.random;

        public float GetMaxLength()
        {
            float maxLength = 0;
            for (int i = 0; i < Branches[i].length; i++)
            {
                float currentLength = Branches[i].GetMaxLength();
                if (currentLength > maxLength)
                    maxLength = currentLength;
            }
            return maxLength + length;
        }
    }
}
