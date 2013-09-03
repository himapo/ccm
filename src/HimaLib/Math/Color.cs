using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Color : IEquatable<Color>
    {
        byte r;
        byte g;
        byte b;
        byte a;

        public byte R { get { return r; } set { r = value; } }
        public byte G { get { return g; } set { g = value; } }
        public byte B { get { return b; } set { b = value; } }
        public byte A { get { return a; } set { a = value; } }

        public Color(int r, int g, int b)
            : this(r, g, b, 255)
        {
        }

        public Color(int r, int g, int b, int a)
        {
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
            this.a = (byte)a;
        }

        public Color(float r, float g, float b)
            : this(r, g, b, 1.0f)
        {
        }

        public Color(float r, float g, float b, float a)
        {
            this.r = (byte)(MathUtil.Clamp(r, 0.0f, 1.0f) * 255);
            this.g = (byte)(MathUtil.Clamp(g, 0.0f, 1.0f) * 255);
            this.b = (byte)(MathUtil.Clamp(b, 0.0f, 1.0f) * 255);
            this.a = (byte)(MathUtil.Clamp(a, 0.0f, 1.0f) * 255);
        }

        public Color(Vector3 v)
            : this(v.X, v.Y, v.Z, 1.0f)
        {
        }

        public Color(Vector4 v)
            : this(v.X, v.Y, v.Z, v.W)
        {
        }

        public static bool operator !=(Color a, Color b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return
                a.R != b.R ||
                a.G != b.G ||
                a.B != b.B ||
                a.A != b.A;
        }

        public static bool operator ==(Color a, Color b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return !(a != b);
        }

        public bool Equals(Color other)
        {
            if (other == null)
            {
                return false;
            }

            return (this == other);
        }

        public override bool Equals(object other)
        {
            if (other == null || !(other is Color))
            {
                return false;
            }

            return this.Equals((Color)other);
        }

        public override int GetHashCode()
        {
            var red = (int)R;
            var green = (int)G;
            var blue = (int)B;
            var alpha = (int)A;

            return (red << 24) | (green << 16) | (blue << 8) | alpha;
        }

        public override string ToString()
        {
            return "{" + String.Format("R:{0:d} G:{1:d} B:{2:d} A:{3:d}", R, G, B, A) + "}";
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)R / 255, (float)G / 255, (float)B / 255);
        }

        public Vector4 ToVector4()
        {
            return new Vector4((float)R / 255, (float)G / 255, (float)B / 255, (float)A / 255);
        }

        public static Color Black { get { return new Color(0, 0, 0); } }
        public static Color Blue { get { return new Color(0, 0, 255); } }
        public static Color Cyan { get { return new Color(0, 255, 255); } }
        public static Color Gray { get { return new Color(128, 128, 128); } }
        public static Color Green { get { return new Color(0, 255, 0); } }
        public static Color LightBlue { get { return new Color(173, 216, 230); } }
        public static Color LightGreen { get { return new Color(144, 238, 144); } }
        public static Color Magenta { get { return new Color(255, 0, 255); } }
        public static Color Purple { get { return new Color(128, 0, 128); } }
        public static Color Red { get { return new Color(255, 0, 0); } }
        public static Color White { get { return new Color(255, 255, 255); } }
        public static Color Yellow { get { return new Color(255, 255, 0); } }

    }
}
