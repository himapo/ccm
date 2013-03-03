using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public static class MathUtil
    {
        public const float E = 2.71828f;
        public const float Log10E = 0.434294f;
        public const float Log2E = 1.4427f;
        public const float Pi = 3.14159f;
        public const float PiOver2 = 1.5708f;
        public const float PiOver4 = 0.785398f;
        public const float TwoPi = 6.28319f;

        public static float Clamp(float value, float min, float max)
        {
            return Min(Max(value, min), max);
        }

        public static float Max(float value1, float value2)
        {
            return value1 < value2 ? value2 : value1;
        }

        public static float Min(float value1, float value2)
        {
            return value1 > value2 ? value2 : value1;
        }

        public static float ToRadians(float degrees)
        {
            return degrees * Pi / 180.0f;
        }

        public static float ToDegrees(float radians)
        {
            return radians * 180.0f / Pi;
        }

        public static float Sin(float radians)
        {
            return (float)global::System.Math.Sin(radians);
        }

        public static float Cos(float radians)
        {
            return (float)global::System.Math.Cos(radians);
        }
    }
}
