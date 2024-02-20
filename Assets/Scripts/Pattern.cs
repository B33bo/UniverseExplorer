using UnityEngine;

namespace Universe
{
    public struct Pattern
    {
        public Color Primary, Secondary, Tertiary;
        public Type patternType;
        public float rotation;
        public Vector2 scale, offset;
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
            if (Primary.ToHumanString() == Secondary.ToHumanString())
            {
                switch (patternType)
                {
                    case Type.LinearGradient:
                    case Type.LinearReflectedGradient:
                    case Type.Dalmation:
                        return Primary.ToHumanString();
                    case Type.Stripes:
                        return Primary.ToHumanString() + "Striped";
                    case Type.Checkerboard:
                        return Primary.ToHumanString() + "Checkered";
                    case Type.LSD:
                        return Primary.ToHumanString() + "Magical";
                    case Type.Mash:
                        return Primary.ToHumanString() + "Tripped";
                    default:
                        break;
                }
            }

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

        public static bool operator ==(Pattern a, Pattern b)
        {
            return a.patternType == b.patternType &&
                a.rotation == b.rotation &&
                a.offset == b.offset &&
                a.scale == b.scale &&
                a.Primary == b.Primary &&
                a.Secondary == b.Secondary &&
                a.Tertiary == b.Tertiary;
        }

        public static bool operator !=(Pattern a, Pattern b) => !(a == b);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void LoadMaterial(Material patternMaterial)
        {
            if (scale == Vector2.zero)
                scale = Vector2.one;

            patternMaterial.SetFloat("_TextureRotate", rotation);
            patternMaterial.SetVector("_TextureOffset", offset);
            patternMaterial.SetVector("_TextureStretch", scale);

            patternMaterial.SetColor("_Red", Primary);
            patternMaterial.SetColor("_Green", Secondary);
            patternMaterial.SetColor("_Blue", Tertiary);

            patternMaterial.SetTexture("_Pattern", Resources.Load<Texture>("Patterns/" + patternType.ToString()));
        }
    }
}
