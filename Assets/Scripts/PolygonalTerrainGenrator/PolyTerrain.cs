using System.Collections.Generic;
using UnityEngine;

namespace Universe.Terrain
{
    public class PolyTerrain : CelestialBody
    {
        public const float RealWidth = 20f;
        public const float Resolution = 1f;
        public override string TypeString => "Terrain";

        public override string TravelTarget => "";

        public override bool Circular => false;

        public Vector2[] points;
        public float height;
        public int ObjectSeed;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            ObjectSeed = RandomNumberGenerator.Next();
        }

        public float HeightAt(float x)
        {
            if (points[0].x > x) return points[0].y;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].x < x)
                    continue;
                if (points[i].x == x)
                    return points[i].y;

                Vector2 p1 = points[i - 1];
                Vector2 p2 = points[i];
                float t = (x - p1.x) / (p2.x - p1.x);
                return (p2.y - p1.y) * t + p1.y;
            }
            return points[points.Length - 1].y;
        }

        public void Create(Vector2 pos, PolyTerrainLayer layer)
        {
            Position = pos;
            height = layer.MinimumHeight;
            ObjectSeed = RandomNumberGenerator.Next();

            float originalPos = pos.x - RealWidth * .5f;
            float endPos = pos.x + RealWidth * .5f;

            float currentPos = originalPos;

            List<Vector2> points = new List<Vector2>((int)(RealWidth / Resolution));

            float oldValue = 0;

            while (currentPos <= endPos)
            {
                points.Add(new Vector2(currentPos - originalPos, layer.HeightAtPoint(currentPos)));

                currentPos += Resolution;
                if (currentPos == oldValue)
                {
                    Debug.LogWarning("Floating point errors whooo?");
                    currentPos = NextAfter.Next(currentPos);
                }

                oldValue = currentPos;
            }

            this.points = points.ToArray();
        }
    }
}
