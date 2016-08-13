using System;

namespace Swiss
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Method generates a random value between two given values
        /// </summary>
        public static double NextDouble(this Random rand, double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }
    }
}
