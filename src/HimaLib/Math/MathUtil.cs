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

        /// <summary>
        /// LUP行列分解
        /// </summary>
        /// <param name="matrix">入出力</param>
        /// <param name="perm">置換情報</param>
        /// <param name="toggle">行列式計算に使うトグル情報</param>
        /// <returns>成否</returns>
        static bool MatrixDecompose(ref float[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition.
            // assumes matrix is square.
            int n = matrix.Length; // convenience
            perm = new int[n];
            for (int i = 0; i < n; ++i) { perm[i] = i; }
            toggle = 1;
            for (int j = 0; j < n - 1; ++j) // each column
            {
                float colMax = global::System.Math.Abs(matrix[j][j]); // largest val in col j
                int pRow = j;
                for (int i = j + 1; i < n; ++i)
                {
                    if (matrix[i][j] > colMax)
                    {
                        colMax = matrix[i][j];
                        pRow = i;
                    }
                }
                if (pRow != j) // swap rows
                {
                    float[] rowPtr = matrix[pRow];
                    matrix[pRow] = matrix[j];
                    matrix[j] = rowPtr;
                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;
                    toggle = -toggle; // row-swap toggle
                }
                if (global::System.Math.Abs(matrix[j][j]) < 1.0E-20)
                {
                    return false;
                }
                for (int i = j + 1; i < n; ++i)
                {
                    matrix[i][j] /= matrix[j][j];
                    for (int k = j + 1; k < n; ++k)
                        matrix[i][k] -= matrix[i][j] * matrix[j][k];
                }
            } // main j column loop
            return true;
        }

        /// <summary>
        /// 逆行列計算のためのヘルパ関数
        /// </summary>
        /// <param name="luMatrix">LU分解済み行列</param>
        /// <param name="b"></param>
        /// <returns></returns>
        static float[] HelperSolve(float[][] luMatrix, float[] b)
        {
            // solve luMatrix * x = b
            int n = luMatrix.Length;
            float[] x = new float[n];
            b.CopyTo(x, 0);
            for (int i = 1; i < n; ++i)
            {
                float sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }
            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                float sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }
            return x;
        }

        /// <summary>
        /// 逆行列を計算する
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float[][] MatrixInverse(float[][] matrix)
        {
            int n = matrix.Length;
            
            var result = new float[n][];
            for (var i = 0; i < n; ++i)
            {
                result[i] = new float[n];
            }

            int[] perm;
            int toggle;
            if (!MatrixDecompose(ref matrix, out perm, out toggle))
            {
                throw new Exception("Unable to compute inverse");
            }

            float[] b = new float[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0f;
                    else
                        b[j] = 0.0f;
                }
                float[] x = HelperSolve(matrix, b);
                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }
    }
}
