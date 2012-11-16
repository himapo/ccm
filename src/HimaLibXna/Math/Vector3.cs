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

        public Vector3(float value)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector3(value);
        }
        
        public Vector3(float x, float y, float z)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector3(x, y, z);
        }

        public bool Equals(Vector3 other)
        {
            return XnaVector.Equals(other.XnaVector);
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
    }
}
