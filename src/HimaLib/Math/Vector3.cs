using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;

        public float Y;

        public float Z;

        public static Vector3 Forward { get { return new Vector3(0.0f, 0.0f, -1.0f); } }

        public static Vector3 One { get { return new Vector3(1.0f); } }

        public static Vector3 UnitX { get { return new Vector3(1.0f, 0.0f, 0.0f); } }

        public static Vector3 UnitY { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        public static Vector3 UnitZ { get { return new Vector3(0.0f, 0.0f, 1.0f); } }

        public static Vector3 Up { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        public static Vector3 Zero { get { return new Vector3(0.0f); } }

        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
        }

        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            return new Vector3(scaleFactor * value.X, scaleFactor * value.Y, scaleFactor * value.Z);
        }

        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            return new Vector3(value.X * scaleFactor, value.Y * scaleFactor, value.Z * scaleFactor);
        }

        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }

        public bool Equals(Vector3 other)
        {
            return (X == other.X && Y == other.Y && Z == other.Z);
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.Y * vector2.Z - vector1.Z * vector2.Y,
                vector1.Z * vector2.X - vector1.X * vector2.Z,
                vector1.X * vector2.Y - vector1.Y * vector2.X);
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return (vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z);
        }

        public float Length()
        {
            return (float)global::System.Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {
            var length = Length();
            X = X / length;
            Y = Y / length;
            Z = Z / length;
        }

        public static Vector3 Transform(Vector3 vector, Matrix matrix)
        {
            return new Vector3(
                vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31,
                vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32,
                vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33);
        }
    }
}
