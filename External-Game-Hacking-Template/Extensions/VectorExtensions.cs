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
            return (float)System.Math.Sqrt(Math.Pow(vect.X, 2) + System.Math.Pow(vect.Y, 2));
        }

        public static float GetDistance3D(Vector3 playerPosition, Vector3 enemyPosition)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(enemyPosition.X - playerPosition.X, 2f) + System.Math.Pow(enemyPosition.Y - playerPosition.Y, 2f) + System.Math.Pow(enemyPosition.Z - playerPosition.Z, 2f)));
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

        /// <summary>
        /// Rotates a Vector2 by x degrees
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
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

        public static Vector3 WithX(this Vector3 parent, float x)
        {
            return new Vector3(x, parent.Y, parent.Z);
        }

        public static Vector3 WithXY(this Vector3 parent, float x, float y)
        {
            return new Vector3(x, y, parent.Z);
        }

        public static Vector3 WithXZ(this Vector3 parent, float x, float z)
        {
            return new Vector3(x, parent.Y, z);
        }

        public static Vector3 WithY(this Vector3 parent, float y)
        {
            return new Vector3(parent.X, y, parent.Z);
        }

        public static Vector3 WithZ(this Vector3 parent, float z)
        {
            return new Vector3(parent.X, parent.Y, z);
        }

        public static Vector3 AddX(this Vector3 parent, float x)
        {
            return new Vector3(parent.X + x, parent.Y, parent.Z);
        }

        public static Vector3 AddY(this Vector3 parent, float y)
        {
            return new Vector3(parent.X, parent.Y + y, parent.Z);
        }

        public static Vector3 AddZ(this Vector3 parent, float z)
        {
            return new Vector3(parent.X, parent.Y, parent.Z + z);
        }

        public static Vector3 FlipX(this Vector3 parent)
        {
            return new Vector3(-parent.X, parent.X, parent.Z);
        }

        public static Vector3 FlipY(this Vector3 parent)
        {
            return new Vector3(parent.X, -parent.Y, parent.Z);
        }

        public static Vector3 FlipZ(this Vector3 parent)
        {
            return new Vector3(parent.X, parent.Y, -parent.Z);
        }

        public static Vector3 AddVector2(this Vector3 parent, Vector2 v2)
        {
            return new Vector3(parent.X + v2.X, parent.Y + v2.Y, parent.Z);
        }

        public static double Distance2D(Vector3 v1, Vector3 v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));

        }

        public static double DistanceXY(Vector3 v1, Vector3 v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));

        }

        public static double DistanceXZ(Vector3 v1, Vector3 v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Z - v2.Z, 2));
        }

        public static double Angle2D(Vector3 v1, Vector3 v2)
        {
            var result = (Math.Atan2(v2.Y - v1.Y, v2.X - v1.X) * 360 / (Math.PI * 2));
            if (result < 0) result += 360;
            if (result > 360) result -= 360;
            return result;
        }

        public static double Angle2DXZ(Vector3 v1, Vector3 v2)
        {
            var result = (Math.Atan2(v2.Z - v1.Z, v2.X - v1.X) * 360 / (Math.PI * 2));
            if (result < 0) result += 360;
            if (result > 360) result -= 360;
            return result;
        }


        public static Vector3 Average(params Vector3[] vectors)
        {
            Vector3 total = Vector3.Zero;
            for (int i = 0; i < vectors.Length; i++)
            {
                total += vectors[i];
            }
            return total / vectors.Length;
        }
    }
}
