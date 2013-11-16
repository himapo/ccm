using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;

namespace HimaLib.Math
{
    public static class MathUtilXna
    {
        public static Microsoft.Xna.Framework.Color ToXnaColor(HimaLib.Math.Color c)
        {
            return new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
        }

        public static Microsoft.Xna.Framework.Vector2 ToXnaVector(HimaLib.Math.Vector2 v)
        {
            return new Microsoft.Xna.Framework.Vector2(v.X, v.Y);
        }

        public static Microsoft.Xna.Framework.Vector3 ToXnaVector(HimaLib.Math.Vector3 v)
        {
            return new Microsoft.Xna.Framework.Vector3(v.X, v.Y, v.Z);
        }

        public static Microsoft.Xna.Framework.Vector4 ToXnaVector(HimaLib.Math.Vector4 v)
        {
            return new Microsoft.Xna.Framework.Vector4(v.X, v.Y, v.Z, v.W);
        }

        public static Microsoft.Xna.Framework.Matrix ToXnaMatrix(HimaLib.Math.Matrix m)
        {
            return new Microsoft.Xna.Framework.Matrix(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44);
        }

        public static Matrix ToHimaLibMatrix(Microsoft.Xna.Framework.Matrix m)
        {
            return new Matrix(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44);
        }

        public static Microsoft.Xna.Framework.Vector2[] CalcSampleOffsets4x4(float srcWidth, float srcHeight)
        {
            var sampleOffsets = MathUtil.CalcSampleOffsets4x4(srcWidth, srcHeight);

            return sampleOffsets.Select((v) =>
            {
                return ToXnaVector(v);
            }).ToArray();
        }
    }
}
