using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Btools.utils;

namespace Btools.numerics
{
    /// <summary>Useful randomizer functions</summary>
    public static class Randomizer
    {
        /// <summary>Get a random item, where one item can be more likely than another</summary>
        /// <param name="weights">the weights of each item.</param>
        /// <returns>Picks a random number from the weight table, and returns the index</returns>
        public static int GetIndexFromWeight(float[] weights, float number)
        {
            float sum = 0;
            foreach (float current in weights)
                sum += current;

            float randomNumber = number;
            float weightTotalCurrent = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                weightTotalCurrent += weights[i];

                if (weightTotalCurrent > randomNumber)
                    return i;
            }

            //Incase it finds it right at the end
            if (weightTotalCurrent >= randomNumber)
                return weights.Length-1;

            //There was some kind of error
            return -1;
        }
    }
}
