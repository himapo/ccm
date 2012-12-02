using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public struct Vector4 : IVector4, IEquatable<Vector4>
    {
        Microsoft.Xna.Framework.Vector4 XnaVector;

        public float W { get { return XnaVector.W; } set { XnaVector.W = value; } }        

        public float X { get { return XnaVector.X; } set { XnaVector.X = value; } }

        public float Y { get { return XnaVector.Y; } set { XnaVector.Y = value; } }

        public float Z { get { return XnaVector.Z; } set { XnaVector.Z = value; } }

        public static Vector4 One { get { return new Vector4(Microsoft.Xna.Framework.Vector4.One); } }

        public static Vector4 UnitW { get { return new Vector4(Microsoft.Xna.Framework.Vector4.UnitW); } }

        public static Vector4 UnitX { get { return new Vector4(Microsoft.Xna.Framework.Vector4.UnitX); } }

        public static Vector4 UnitY { get { return new Vector4(Microsoft.Xna.Framework.Vector4.UnitY); } }

        public static Vector4 UnitZ { get { return new Vector4(Microsoft.Xna.Framework.Vector4.UnitZ); } }

        public static Vector4 Zero { get { return new Vector4(Microsoft.Xna.Framework.Vector4.Zero); } }

        public static Microsoft.Xna.Framework.Vector4 CreateXnaVector(IVector4 vector)
        {
            return new Microsoft.Xna.Framework.Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public Vector4(float value)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector4(value);
        }
        
        public Vector4(float x, float y, float z, float w)
        {
            XnaVector = new Microsoft.Xna.Framework.Vector4(x, y, z, w);
        }

        public Vector4(Microsoft.Xna.Framework.Vector4 xnaVector)
        {
            XnaVector = xnaVector;
        }

        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1.XnaVector - value2.XnaVector);
        }

        public bool Equals(Vector4 other)
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

        public static Vector4 Transform(Vector4 vector, Matrix matrix)
        {
            return new Vector4(
                Microsoft.Xna.Framework.Vector4.Transform(
                    Vector4.CreateXnaVector(vector),
                    Matrix.CreateXnaMatrix(matrix)));
        }
    }
}
