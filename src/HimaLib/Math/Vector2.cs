using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;

        public float Y;

        public static Vector2 One { get { return new Vector2(1.0f); } }

        public static Vector2 UnitX { get { return new Vector2(1.0f, 0.0f); } }

        public static Vector2 UnitY { get { return new Vector2(0.0f, 1.0f); } }

        public static Vector2 Zero { get { return new Vector2(0.0f); } }

        public Vector2(float value)
        {
            X = Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator -(Vector2 value)
        {
            return new Vector2(-value.X, -value.Y);
        }

        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            return new Vector2(value1.X - value2.X, value1.Y - value2.Y);
        }

        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            return new Vector2(scaleFactor * value.X, scaleFactor * value.Y);
        }

        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            return new Vector2(value.X * scaleFactor, value.Y * scaleFactor);
        }

        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            return new Vector2(value1.X + value2.X, value1.Y + value2.Y);
        }

        public bool Equals(Vector2 other)
        {
            return (X == other.X && Y == other.Y);
        }

        public static float Dot(Vector2 vector1, Vector2 vector2)
        {
            return (vector1.X * vector2.X + vector1.Y * vector2.Y);
        }

        public float Length()
        {
            return (float)global::System.Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (X * X + Y * Y);
        }

        public void Normalize()
        {
            var length = Length();
            X = X / length;
            Y = Y / length;
        }
    }
}
