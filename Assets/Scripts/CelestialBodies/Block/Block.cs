using System.Collections.Generic;
using UnityEngine;

namespace Universe
{
    public abstract class Block : CelestialBodyRenderer
    {
        public Vector2 BlockScale;
        public abstract string BlockName { get; }
        public abstract string Path { get; }
        public virtual string ScenePath { get => string.Empty; }

        public static Block blockSelected;
        private static Dictionary<Vector2Int, object> getSaveData = new Dictionary<Vector2Int, object>();

        public override void Spawn(Vector2 pos, int? seed)
        {
            BlockScale = new Vector2(1, 1);

            Target = new BlockObject
            {
                Position = pos,
                Width = BlockScale.x * Measurement.M,
                Height = BlockScale.y * Measurement.M,
                Name = BlockName,
                blockTravelTarget = ScenePath,
            };

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            if (cameraLerpTarget is null)
            {
                cameraLerpTarget = transform;
                cameraLerpSize = 1;
                cameraLerpMultiplyBySize = true;
            }
        }

        private void Update()
        {
            if (blockSelected != this)
                return;

            if (Input.GetMouseButtonDown(0))
                DestroyBlock();
        }

        private void DestroyBlock()
        {
            if (blockSelected == this)
                blockSelected = null;

            //if (getSaveData.ContainsKey((Vector2Int)Target.Position))
            Destroy(gameObject);
        }

        public abstract void BlockCreate(Vector2 pos);

        public virtual SaveData GetSaveData()
        {
            SaveData saveData = new SaveData
            {
                Position = Target.Position,
            };

            return saveData;
        }

        private void OnMouseDown()
        {
            
        }

        private void OnMouseEnter()
        {
            blockSelected = this;
        }

        private void OnMouseExit()
        {
            if (blockSelected != this)
                return;
            blockSelected = null;
        }

        public class SaveData
        {
            public Vector2 Position;
        }
    }
}
