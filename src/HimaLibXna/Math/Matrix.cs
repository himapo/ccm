using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public struct Matrix : IMatrix, IEquatable<Matrix>
    {
        Microsoft.Xna.Framework.Matrix XnaMatrix;

        public float M11 { get { return XnaMatrix.M11; } set { XnaMatrix.M11 = value; } }
        public float M12 { get { return XnaMatrix.M12; } set { XnaMatrix.M12 = value; } }
        public float M13 { get { return XnaMatrix.M13; } set { XnaMatrix.M13 = value; } }
        public float M14 { get { return XnaMatrix.M14; } set { XnaMatrix.M14 = value; } }
        public float M21 { get { return XnaMatrix.M21; } set { XnaMatrix.M21 = value; } }
        public float M22 { get { return XnaMatrix.M22; } set { XnaMatrix.M22 = value; } }
        public float M23 { get { return XnaMatrix.M23; } set { XnaMatrix.M23 = value; } }
        public float M24 { get { return XnaMatrix.M24; } set { XnaMatrix.M24 = value; } }
        public float M31 { get { return XnaMatrix.M31; } set { XnaMatrix.M31 = value; } }
        public float M32 { get { return XnaMatrix.M32; } set { XnaMatrix.M32 = value; } }
        public float M33 { get { return XnaMatrix.M33; } set { XnaMatrix.M33 = value; } }
        public float M34 { get { return XnaMatrix.M34; } set { XnaMatrix.M34 = value; } }
        public float M41 { get { return XnaMatrix.M41; } set { XnaMatrix.M41 = value; } }
        public float M42 { get { return XnaMatrix.M42; } set { XnaMatrix.M42 = value; } }
        public float M43 { get { return XnaMatrix.M43; } set { XnaMatrix.M43 = value; } }
        public float M44 { get { return XnaMatrix.M44; } set { XnaMatrix.M44 = value; } }

        public static Matrix Identity { get { return new Matrix(Microsoft.Xna.Framework.Matrix.Identity); } }

        public static Microsoft.Xna.Framework.Matrix CreateXnaMatrix(IMatrix matrix)
        {
            return new Microsoft.Xna.Framework.Matrix(
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44);
        }

        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            XnaMatrix = new Microsoft.Xna.Framework.Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m44, m44);
        }

        public Matrix(Microsoft.Xna.Framework.Matrix xnaMatrix)
        {
            XnaMatrix = xnaMatrix;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            return new Matrix(matrix1.XnaMatrix * matrix2.XnaMatrix);
        }

        public bool Equals(Matrix other)
        {
            return XnaMatrix.Equals(other.XnaMatrix);
        }

        public static Matrix CreateScale(float scale)
        {
            return new Matrix(Microsoft.Xna.Framework.Matrix.CreateScale(scale));
        }

        public static Matrix CreateRotationX(float radians)
        {
            return new Matrix(Microsoft.Xna.Framework.Matrix.CreateRotationX(radians));
        }

        public static Matrix CreateRotationY(float radians)
        {
            return new Matrix(Microsoft.Xna.Framework.Matrix.CreateRotationY(radians));
        }

        public static Matrix CreateRotationZ(float radians)
        {
            return new Matrix(Microsoft.Xna.Framework.Matrix.CreateRotationZ(radians));
        }

        public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
        {
            return new Matrix(Microsoft.Xna.Framework.Matrix.CreateTranslation(xPosition, yPosition, zPosition));
        }
    }
}
