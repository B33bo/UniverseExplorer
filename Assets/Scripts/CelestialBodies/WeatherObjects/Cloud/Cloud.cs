using UnityEngine;

namespace Universe.CelestialBodies
{
    public class Cloud : CelestialBody
    {
        public override string TypeString => "Cloud";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Vector3[] points;
        public int pointWidth;
        public float spikyness;

        public override void Create(Vector2 pos)
        {
            Position = pos;

            pointWidth = RandomNum.Get(4, 10, RandomNumberGenerator);
            points = new Vector3[pointWidth * 2 + 2];

            points[0] = new Vector2(-.2f - RandomNum.GetFloat(.5f, RandomNumberGenerator), 0);
            points[pointWidth + 1] = new Vector2(.2f + RandomNum.GetFloat(.5f, RandomNumberGenerator), 0);

            for (int i = 0; i < pointWidth; i++)
            {
                float x = i / (pointWidth - 1f) - .5f;
                points[i + 1] = new Vector2(x, .2f + RandomNum.GetFloat(.5f, RandomNumberGenerator));
                points[points.Length - i - 1] = new Vector3(x, -.2f - RandomNum.GetFloat(.5f, RandomNumberGenerator));
            }
        }
    }
}
