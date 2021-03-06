﻿using System;
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

        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return (value1.X != value2.X) || (value1.Y != value2.Y) || (value1.Z != value2.Z);
        }

        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return !(value1 != value2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector3))
            {
                return false;
            }

            return Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other)
        {
            if (other == null)
            {
                return false;
            }

            return (this == other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        /// <summary>
        /// w=1とした同次座標変換
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector4 Transform(Vector3 vector, Matrix matrix)
        {
            return Vector4.Transform(
                new Vector4(vector.X, vector.Y, vector.Z, 1.0f),
                matrix);
        }

        /// <summary>
        /// 同次座標をw除算して返す
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector3 TransformCoord(Vector3 vector, Matrix matrix)
        {
            var v = Transform(vector, matrix);
            return new Vector3(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        /// <summary>
        /// w除算を省略したアフィン変換専用のトランスフォーム
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix">アフィン変換行列。すなわち4列目が(0,0,0,1)であること</param>
        /// <returns></returns>
        public static Vector3 TransformAffine(Vector3 vector, Matrix matrix)
        {
            return new Vector3(
                vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41,
                vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42,
                vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43);
        }

        /// <summary>
        /// w=0として平行移動成分を除去して変換する
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector3 TransformNormal(Vector3 vector, Matrix matrix)
        {
            var v = Vector4.Transform(
                new Vector4(vector.X, vector.Y, vector.Z, 0.0f),
                matrix);
            return new Vector3(v.X, v.Y, v.Z);
        }
    }
}
