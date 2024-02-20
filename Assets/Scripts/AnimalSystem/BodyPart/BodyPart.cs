using System;

namespace Universe.Animals
{
    public abstract class BodyPart
    {
        public static void InitBodyParts<T>(T[] parts, Action<int, T> action) where T : BodyPart
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = Activator.CreateInstance<T>();
                action.Invoke(i, parts[i]);
            }
        }

        public static void InitBodyParts<T>(T[] parts) where T : BodyPart
        {
            InitBodyParts(parts, (_, _) => { });
        }
    }

    public abstract class BodyPartRenderer<T>
    {
        public abstract void Init(T target);
    }

    public class Ear : BodyPart
    {

    }

    public class Nose : BodyPart
    {
        public Pattern pattern;
    }

    public class Torso : BodyPart
    {
        public Pattern pattern;
    }

    public class Tail : BodyPart
    {
        public Pattern pattern;
        public float length;
        public float wagSpeed;
    }
}
