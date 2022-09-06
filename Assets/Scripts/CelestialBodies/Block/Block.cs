using UnityEngine;

namespace Universe
{
    public abstract class Block : CelestialBodyRenderer
    {
        public Vector2 BlockScale;
        public abstract string BlockName { get; }
        public abstract string Path { get; }

        public override void Spawn(Vector2 pos, int? seed)
        {
            BlockScale = new Vector2(1, 1);

            Target = new BlockObject
            {
                Position = pos,
                Width = BlockScale.x * Measurement.M,
                Height = BlockScale.y * Measurement.M
            };

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }

        public abstract void BlockCreate(Vector2 pos);

        public virtual string GetSaveData()
        {
            SaveData saveData = new SaveData
            {
                Position = Target.Position,
            };

            return JsonUtility.ToJson(saveData);
        }

        public class SaveData
        {
            public Vector2 Position;
        }
    }
}
