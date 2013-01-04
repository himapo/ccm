using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public struct Vector3 : IVector3, IEquatable<Vector3>
    {
        Microsoft.Xna.Framework.Vector3 XnaVector;

        public float X { get { return XnaVector.X; } set { XnaVector.X = value; } }

        public float Y { get { return XnaVector.Y; } set { XnaVector.Y = value; } }

        public float Z { get { return XnaVector.Z; } set { XnaVector.Z = value; } }

        public static Vector3 Forward { get { return new Vector3(0.0f, 0.0f, -1.0f); } }

        public static Vector3 One { get { return new Vector3(1.0f); } }

        public static Vector3 UnitX { get { return new Vector3(1.0f, 0.0f, 0.0f); } }

        public static Vector3 UnitY { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        public static Vector3 UnitZ { get { return new Vector3(0.0f, 0.0f, 1.0f); } }

        public static Vector3 Up { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        public static Vector3 Zero { get { return new Vector3(0.0f); } }

        public static Microsoft.Xna.Framework.Vector3 CreateXnaVector(IVector3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }

        public Vector3(float value)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector3(value);
        }
        
        public Vector3(float x, float y, float z)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector3(x, y, z);
        }

        public Vector3(Microsoft.Xna.Framework.Vector3 xnaVector)
        {
            XnaVector = xnaVector;
        }

        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.XnaVector - value2.XnaVector);
        }

        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            return new Vector3(scaleFactor * value.XnaVector);
        }

        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            return new Vector3(value.XnaVector * scaleFactor);
        }

        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.XnaVector + value2.XnaVector);
        }

        public bool Equals(Vector3 other)
        {
            return XnaVector.Equals(other.XnaVector);
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(Microsoft.Xna.Framework.Vector3.Cross(vector1.XnaVector, vector2.XnaVector));
        }

        public float Length()
        {
            return XnaVector.Length();
        }

        public float LengthSquared()
        {
            return XnaVector.LengthSquared();
        }

        public void Normalize()
        {
            XnaVector.Normalize();
        }

        public static Vector3 Transform(Vector3 vector, Matrix matrix)
        {
            return new Vector3(
                Microsoft.Xna.Framework.Vector3.Transform(
                    Vector3.CreateXnaVector(vector),
                    Matrix.CreateXnaMatrix(matrix)));
        }
    }
}
