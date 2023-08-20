using UnityEngine;

namespace Universe
{
    public struct Pattern
    {
        public Color Primary, Secondary, Tertiary;
        public Type patternType;
        public float rotation;
        public short id;

        public enum Type
        {
            Solid,
            LinearGradient,
            LinearReflectedGradient,
            CircleGradient,
            DiamondGradient,
            SpiralGradient,
            Stripes,
            Spots,
            Scales,
            Perlin,
            Worley,
            Cracks,
            Checkerboard,
            Dalmation,
            LSD,
            Mash,
        }

        public override string ToString()
        {
            return patternType switch
            {
                Type.Solid => Primary.ToHumanString(),
                Type.LinearGradient => Primary.ToHumanString() + " and " + Secondary.ToHumanString(),
                Type.LinearReflectedGradient => Primary.ToHumanString() + " and " + Secondary.ToHumanString(),
                Type.CircleGradient => Primary.ToHumanString() + " Faded",
                Type.DiamondGradient => Primary.ToHumanString() + " Diamond",
                Type.SpiralGradient => Secondary.ToHumanString() + " Spiraled",
                Type.Stripes => Primary.ToHumanString() + " and " + Secondary.ToHumanString() + " Striped",
                Type.Spots => Secondary.ToHumanString() + " Spotted",
                Type.Scales => Secondary.ToHumanString() + " Scaled",
                Type.Perlin => Secondary.ToHumanString() + " Cloudy",
                Type.Worley => Secondary.ToHumanString() + " Cellular",
                Type.Cracks => Primary.ToHumanString() + " Cracked",
                Type.Checkerboard => Primary.ToHumanString() + " and " + Secondary.ToHumanString() + " Checkered",
                Type.Dalmation => Primary.ToHumanString() + " and " + Secondary.ToHumanString(),
                Type.LSD => Primary.ToHumanString() + " and " + Secondary.ToHumanString() + " Magical",
                Type.Mash => Primary.ToHumanString() + " and " + Secondary.ToHumanString() + " Tripped",
                _ => Primary.ToHumanString() + " and " + Secondary.ToHumanString(),
            };
        }
    }
}
