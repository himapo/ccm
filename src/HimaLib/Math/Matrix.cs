using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Matrix : IEquatable<Matrix>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        public static Matrix Identity
        {
            get
            {
                return new Matrix(
                    1.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, 1.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 1.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 1.0f);
            }
        }

        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            return new Matrix(
                matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41,
                matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42,
                matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43,
                matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44,
                matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41,
                matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42,
                matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43,
                matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44,
                matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41,
                matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42,
                matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43,
                matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44,
                matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41,
                matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42,
                matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43,
                matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44);
        }

        public static Matrix operator /(Matrix matrix1, float divider)
        {
            return new Matrix(
                matrix1.M11 / divider, matrix1.M12 / divider, matrix1.M13 / divider, matrix1.M14 / divider,
                matrix1.M21 / divider, matrix1.M22 / divider, matrix1.M23 / divider, matrix1.M24 / divider,
                matrix1.M31 / divider, matrix1.M32 / divider, matrix1.M33 / divider, matrix1.M34 / divider,
                matrix1.M41 / divider, matrix1.M42 / divider, matrix1.M43 / divider, matrix1.M44 / divider);
        }

        public bool Equals(Matrix other)
        {
            return false;
        }

        public static Matrix CreateScale(float scale)
        {
            return CreateScale(scale, scale, scale);
        }

        public static Matrix CreateScale(float xScale, float yScale, float zScale)
        {
            return new Matrix(
                xScale, 0.0f, 0.0f, 0.0f,
                0.0f, yScale, 0.0f, 0.0f,
                0.0f, 0.0f, zScale, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix CreateScale(Vector3 vector)
        {
            return CreateScale(vector.X, vector.Y, vector.Z);
        }

        public static Matrix CreateRotationX(float radians)
        {
            return new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, (float)global::System.Math.Cos(radians), (float)global::System.Math.Sin(radians), 0.0f,
                0.0f, (float)-global::System.Math.Sin(radians), (float)global::System.Math.Cos(radians), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix CreateRotationY(float radians)
        {
            return new Matrix(
                (float)global::System.Math.Cos(radians), 0.0f, (float)-global::System.Math.Sin(radians), 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                (float)global::System.Math.Sin(radians), 0.0f, (float)global::System.Math.Cos(radians), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix CreateRotationZ(float radians)
        {
            return new Matrix(
                (float)global::System.Math.Cos(radians), (float)global::System.Math.Sin(radians), 0.0f, 0.0f,
                (float)-global::System.Math.Sin(radians), (float)global::System.Math.Cos(radians), 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix CreateRotation(Vector4 vector)
        {
            return
                CreateRotationZ(vector.Z) *
                CreateRotationY(vector.Y) *
                CreateRotationX(vector.X);
        }

        public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
        {
            return new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                xPosition, yPosition, zPosition, 1.0f);
        }

        public static Matrix CreateTranslation(Vector3 vector)
        {
            return CreateTranslation(vector.X, vector.Y, vector.Z);
        }

        public static Matrix CreateLookAt(Vector3 eye, Vector3 at, Vector3 up)
        {
            var zaxis = eye - at;
            zaxis.Normalize();
            var xaxis = Vector3.Cross(up, zaxis);
            xaxis.Normalize();
            var yaxis = Vector3.Cross(zaxis, xaxis);

            return new Matrix(
                xaxis.X, yaxis.X, zaxis.X, 0.0f,
                xaxis.Y, yaxis.Y, zaxis.Y, 0.0f,
                xaxis.Z, yaxis.Z, zaxis.Z, 0.0f,
                -Vector3.Dot(xaxis, eye), -Vector3.Dot(yaxis, eye), -Vector3.Dot(zaxis, eye), 1.0f);
        }

        public static Matrix CreatePerspectiveFieldOfView(float fov, float aspect, float near, float far)
        {
            var yScale = (float)(1.0 / global::System.Math.Tan(fov / 2.0));
            var xScale = yScale / aspect;

            return new Matrix(
                xScale, 0.0f, 0.0f, 0.0f,
                0.0f, yScale, 0.0f, 0.0f,
                0.0f, 0.0f, far / (near - far), -1.0f,
                0.0f, 0.0f, near * far / (near - far), 0.0f);
        }

        public static Matrix CreateOrthographic(float width, float height, float near, float far)
        {
            return new Matrix(
                2.0f / width, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / height, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f / (near - far), 0.0f,
                0.0f, 0.0f, near / (near - far), 1.0f);
        }

        public static Matrix Invert(Matrix matrix)
        {
            var det = matrix.Determinant();

            var b = new float[4][];

            for (var i = 0; i < 4; ++i)
            {
                matrix.InvertHelper(i + 1, out b[i]);
            }

            return new Matrix(
                b[0][0] / det, b[0][1] / det, b[0][2] / det, b[0][3] / det,
                b[1][0] / det, b[1][1] / det, b[1][2] / det, b[1][3] / det,
                b[2][0] / det, b[2][1] / det, b[2][2] / det, b[2][3] / det,
                b[3][0] / det, b[3][1] / det, b[3][2] / det, b[3][3] / det);
        }

        public float Determinant()
        {
            float[] b;
            InvertHelper(1, out b);
            var d1 = M11 * b[0] + M21 * b[1] + M31 * b[2] + M41 * b[3];
            //InvertHelper(2, out b);
            //var d2 = M12 * b[0] + M22 * b[1] + M32 * b[2] + M42 * b[3];
            //InvertHelper(3, out b);
            //var d3 = M13 * b[0] + M23 * b[1] + M33 * b[2] + M43 * b[3];
            //InvertHelper(4, out b);
            //var d4 = M14 * b[0] + M24 * b[1] + M34 * b[2] + M44 * b[3];
            return d1;
        }

        /// <summary>
        /// row行目の余因子展開
        /// </summary>
        /// <param name="row"></param>
        /// <param name="b"></param>
        void InvertHelper(int row, out float[] b)
        {
            b = new float[4];

            switch (row)
            {
                case 1:
                    b[0] =  (M22 * (M33 * M44 - M34 * M43) + M23 * (M34 * M42 - M32 * M44) + M24 * (M32 * M43 - M33 * M42));
                    b[1] = -(M32 * (M43 * M14 - M44 * M13) + M33 * (M44 * M12 - M42 * M14) + M34 * (M42 * M13 - M43 * M12));
                    b[2] =  (M42 * (M13 * M24 - M14 * M23) + M43 * (M14 * M22 - M12 * M24) + M44 * (M12 * M23 - M13 * M22));
                    b[3] = -(M12 * (M23 * M34 - M24 * M33) + M13 * (M24 * M32 - M22 * M34) + M14 * (M22 * M33 - M23 * M32));                  
                    break;
                case 2:
                    b[0] = -(M23 * (M34 * M41 - M31 * M44) + M24 * (M31 * M43 - M33 * M41) + M21 * (M33 * M44 - M34 * M43));
                    b[1] =  (M33 * (M44 * M11 - M41 * M14) + M34 * (M41 * M13 - M43 * M11) + M31 * (M43 * M14 - M44 * M13));
                    b[2] = -(M43 * (M14 * M21 - M11 * M24) + M44 * (M11 * M23 - M13 * M21) + M41 * (M13 * M24 - M14 * M23));
                    b[3] =  (M13 * (M24 * M31 - M21 * M34) + M14 * (M21 * M33 - M23 * M31) + M11 * (M23 * M34 - M24 * M33));
                    break;
                case 3:
                    b[0] =  (M24 * (M31 * M42 - M32 * M41) + M21 * (M32 * M44 - M34 * M42) + M22 * (M34 * M41 - M31 * M44));
                    b[1] = -(M34 * (M41 * M12 - M42 * M11) + M31 * (M42 * M14 - M44 * M12) + M32 * (M44 * M11 - M41 * M14));
                    b[2] =  (M44 * (M11 * M22 - M12 * M21) + M41 * (M12 * M24 - M14 * M22) + M42 * (M14 * M21 - M11 * M24));
                    b[3] = -(M14 * (M21 * M32 - M22 * M31) + M11 * (M22 * M34 - M24 * M32) + M12 * (M24 * M31 - M21 * M34));
                    break;
                case 4:
                    b[0] = -(M21 * (M32 * M43 - M33 * M42) + M22 * (M33 * M41 - M31 * M43) + M23 * (M31 * M42 - M32 * M41));
                    b[1] =  (M31 * (M42 * M13 - M43 * M12) + M32 * (M43 * M11 - M41 * M13) + M33 * (M41 * M12 - M42 * M11));
                    b[2] = -(M41 * (M12 * M23 - M13 * M22) + M42 * (M13 * M21 - M11 * M23) + M43 * (M11 * M22 - M12 * M21));
                    b[3] =  (M11 * (M22 * M33 - M23 * M32) + M12 * (M23 * M31 - M21 * M33) + M13 * (M21 * M32 - M22 * M31));
                    break;
                default:
                    break;
            }
        }
    }
}
