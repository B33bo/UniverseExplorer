using UnityEngine;

namespace Universe.Animals
{
    public class Leg : BodyPart
    {
        public float length = 1;
        public Pattern pattern;

        [System.Serializable]
        public class LegRenderer : BodyPartRenderer<Leg>
        {
            public Transform legScale;
            public SpriteRenderer[] parts;

            public override void Init(Leg target)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    target.pattern.LoadMaterial(parts[i].material);
                }

                legScale.transform.localScale *= new Vector2(1, target.length);
            }
        }
    }
}
