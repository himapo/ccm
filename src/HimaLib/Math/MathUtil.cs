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

        public static int Clamp(int value, int min, int max)
        {
            return Min(Max(value, min), max);
        }

        public static float Max(float value1, float value2)
        {
            return value1 < value2 ? value2 : value1;
        }

        public static int Max(int value1, int value2)
        {
            return value1 < value2 ? value2 : value1;
        }

        public static float Min(float value1, float value2)
        {
            return value1 > value2 ? value2 : value1;
        }

        public static int Min(int value1, int value2)
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

        public static float Tan(float radians)
        {
            return (float)global::System.Math.Tan(radians);
        }

        public static float Cot(float radians)
        {
            return 1.0f / Tan(radians);
        }

        public static float Abs(float value)
        {
            return global::System.Math.Abs(value);
        }

        /// <summary>
        /// ワールド座標をスクリーン座標に変換
        /// </summary>
        /// <param name="position"></param>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <returns></returns>
        public static Vector2 Project(Vector3 position, Matrix view, Matrix projection, int screenWidth, int screenHeight)
        {
            var screenPosition = Vector3.TransformCoord(position, view * projection);

            var x = (1.0f + screenPosition.X) * (screenWidth / 2);
            var y = (1.0f - screenPosition.Y) * (screenHeight / 2);

            return new Vector2(x, y);
        }

        /// <summary>
        /// 4x4のダウンサンプリングをするときのサンプリング座標を求める
        /// </summary>
        /// <param name="srcWidth"></param>
        /// <param name="srcHeight"></param>
        /// <returns></returns>
        public static Vector2[] CalcSampleOffsets4x4(float srcWidth, float srcHeight)
        {
            var result = new List<Vector2>();

            float tU = 1.0f / srcWidth;
            float tV = 1.0f / srcHeight;

            int index = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    result.Add(new Vector2((x - 1.5f) * tU, (y - 1.5f) * tV));
                    index++;
                }
            }

            return result.ToArray();
        }
    }
}
