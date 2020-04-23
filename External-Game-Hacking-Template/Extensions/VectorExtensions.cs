using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace External_Game_Hacking_Template.Extensions
{
    internal static class VectorExtensions
    {
        public static float AngleBetweenVectors(this Vector3 src, Vector3 dest)
        {
            var dotProduct = Vector3.Dot(src, dest);
            var srcMag = Magnitude(src);
            var destMag = Magnitude(dest);
            return (float)System.Math.Acos(dotProduct / (srcMag * destMag));
        }

        public static float Magnitude(Vector3 vect)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(vect.X, 2) + System.Math.Pow(vect.Y, 2));
        }

        public static float GetDistance3D(Vector3 playerPosition, Vector3 enemyPosition)
        {
            return Convert.ToSingle(System.Math.Sqrt(System.Math.Pow(enemyPosition.X - playerPosition.X, 2f) + System.Math.Pow(enemyPosition.Y - playerPosition.Y, 2f) + System.Math.Pow(enemyPosition.Z - playerPosition.Z, 2f)));
        }

        public static Vector3 AngleVector(this Vector3 input)
        {
            var sinY = System.Math.Sin(input.Y / 180f * System.Math.PI);
            var sinX = System.Math.Sin(input.X / 180f * System.Math.PI);

            var cosY = System.Math.Cos(input.Y / 180f * System.Math.PI);
            var cosX = System.Math.Cos(input.X / 180f * System.Math.PI);
            var out1 = cosX * cosY;
            var out2 = cosX * sinY;
            var out3 = -sinX;
            return new Vector3((float)out1, (float)out2, (float)out3);
        }

        /// <inheritdoc cref="Vector3.Normalize(Vector3)" />
        public static Vector3 Normalized(this Vector3 value)
        {
            return Vector3.Normalize(value);
        }

        /// <summary>
        ///     Check if vector is valid to draw in screen space.
        /// </summary>
        public static bool IsValidWorld(this Vector3 value)
        {
            return !value.X.IsInfinityOrNaN() && !value.Y.IsInfinityOrNaN() && !value.Z.IsInfinityOrNaN();
        }

        /// <summary>
        ///     Check if vector is valid to draw in screen space.
        /// </summary>
        public static bool IsValidScreen(this Vector2 value)
        {
            return !value.X.IsInfinityOrNaN() && !value.Y.IsInfinityOrNaN() && value.X != 0f && value.Y != 0f;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            var sin = System.Math.Sin(MathExtensions.ConvertDegreesToRadians(degrees));
            var cos = System.Math.Cos(MathExtensions.ConvertDegreesToRadians(degrees));

            var tx = v.X;
            var ty = v.Y;
            v.X = (float)((cos * tx) - (sin * ty));
            v.Y = (float)((sin * tx) + (cos * ty));
            return v;
        }
    }
}
