using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Vector4 : IEquatable<Vector4>
    {
        public float W;

        public float X;

        public float Y;

        public float Z;

        public static Vector4 One { get { return new Vector4(1.0f); } }

        public static Vector4 UnitW { get { return new Vector4(0.0f, 0.0f, 0.0f, 1.0f); } }

        public static Vector4 UnitX { get { return new Vector4(1.0f, 0.0f, 0.0f, 0.0f); } }

        public static Vector4 UnitY { get { return new Vector4(0.0f, 1.0f, 0.0f, 0.0f); } }

        public static Vector4 UnitZ { get { return new Vector4(0.0f, 0.0f, 1.0f, 0.0f); } }

        public static Vector4 Zero { get { return new Vector4(0.0f); } }

        public Vector4(float value)
        {
            X = Y = Z = W = value;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
            return new Vector4(
                value1.X - value2.X,
                value1.Y - value2.Y,
                value1.Z - value2.Z,
                value1.W - value2.W);
        }

        public bool Equals(Vector4 other)
        {
            return (X == other.X && Y == other.Y && Z == other.Z && W == other.W);
        }

        public static float Dot(Vector4 vector1, Vector4 vector2)
        {
            return (vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W);
        }

        public float Length()
        {
            return (float)global::System.Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (X * X + Y * Y + Z * Z + W * W);
        }

        public void Normalize()
        {
            var length = Length();
            X = X / length;
            Y = Y / length;
            Z = Z / length;
            W = W / length;
        }

        public static Vector4 Transform(Vector4 vector, Matrix matrix)
        {
            return new Vector4(
                vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41,
                vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42,
                vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43,
                vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);
        }
    }
}
