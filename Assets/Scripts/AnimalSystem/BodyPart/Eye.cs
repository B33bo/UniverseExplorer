using UnityEngine;

namespace Universe.Animals
{
    public class Eye : BodyPart
    {
        public Color IrisColor = Color.blue;
        public float Strength = 3;
        public float Radius;

        [System.Serializable]
        public class Renderer : BodyPartRenderer<Eye>
        {
            public SpriteRenderer iris;
            public Transform pupil;

            public override void Init(Eye eye)
            {
                iris.color = eye.IrisColor;

                float eyeScale = Mathf.Min(eye.Strength / 6f, 1);
                pupil.localScale *= eyeScale;
            }
        }
    }
}
