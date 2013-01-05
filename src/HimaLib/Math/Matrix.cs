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
    }
}
