using System;
using UnityEngine;

namespace Universe
{
    public class NextAfter : MonoBehaviour
    {
        public static float Next(float num)
        {
            int floatToInt = BitConverter.ToInt32(BitConverter.GetBytes(num), 0);

            if (num < 0)
                floatToInt--;
            else
                floatToInt++;

            return BitConverter.ToSingle(BitConverter.GetBytes(floatToInt), 0);
        }

        public static float NextSigned(float num)
        {
            int floatToInt = BitConverter.ToInt32(BitConverter.GetBytes(num), 0);
            floatToInt++;
            return BitConverter.ToSingle(BitConverter.GetBytes(floatToInt), 0);
        }

        public static float Previous(float num)
        {
            int floatToInt = BitConverter.ToInt32(BitConverter.GetBytes(num), 0);

            if (num < 0)
                floatToInt++;
            else
                floatToInt--;

            return BitConverter.ToSingle(BitConverter.GetBytes(floatToInt), 0);
        }

        public static float Change(float num, int change)
        {
            int floatToInt = BitConverter.ToInt32(BitConverter.GetBytes(num), 0);

            if (num < 0)
                floatToInt -= change;
            else
                floatToInt += change;

            return BitConverter.ToSingle(BitConverter.GetBytes(floatToInt), 0);
        }
    }
}
