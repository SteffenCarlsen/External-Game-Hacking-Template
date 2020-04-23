using System;
using System.Collections.Generic;
using System.Text;

namespace External_Game_Hacking_Template.Extensions
{
    internal static class MathExtensions
    {
        /// <summary>
        ///     Accepts a value and a range of the value together with another range
        ///     Then it maps the value to the second range
        /// </summary>
        /// <param name="inputVal">Value to map to new range</param>
        /// <param name="inputMin">Input range minimum</param>
        /// <param name="inputMax">Input range maximum</param>
        /// <param name="outputMin">Output range minimum</param>
        /// <param name="outputMax">Output range maximum</param>
        /// <returns></returns>
        public static float Map(float inputVal, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            return (inputVal - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
        }

        public static float HypoF(float x, float y)
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        /// <summary>
        /// Source engine conversion from meters to units
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static float MetersToUnits(float dist)
        {
            return dist / 0.01905f;
        }

        /// <summary>
        /// Source engine conversions from units to meters
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static float DistToMeters(float dist)
        {
            return dist * 0.01905f;
        }

        public static float ConvertRadiansToDegrees(float radians)
        {
            var degrees = 180f / Math.PI * radians;
            return (float)degrees;
        }

        public static double ConvertDegreesToRadians(float degrees)
        {
            var radians = Math.PI / 180 * degrees;
            return radians;
        }

        public static bool IsInfinityOrNaN(this float value)
        {
            return Single.IsNaN(value) || Single.IsInfinity(value);
        }
    }
}
