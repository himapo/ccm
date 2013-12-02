using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Color : IEquatable<Color>
    {
        float r;
        float g;
        float b;
        float a;

        public float R { get { return r; } set { r = value; } }
        public float G { get { return g; } set { g = value; } }
        public float B { get { return b; } set { b = value; } }
        public float A { get { return a; } set { a = value; } }

        public byte LDR_R { get { return ToLDR(r); } set { r = ToHDR(value); } }
        public byte LDR_G { get { return ToLDR(g); } set { g = ToHDR(value); } }
        public byte LDR_B { get { return ToLDR(b); } set { b = ToHDR(value); } }
        public byte LDR_A { get { return ToLDR(a); } set { a = ToHDR(value); } }

        static byte ToLDR(float hdr)
        {
            return (byte)MathUtil.Clamp(hdr * 255.0f, 0.0f, 255.0f);
        }

        static float ToHDR(int ldr)
        {
            return (float)ldr / 255.0f;
        }

        public Color(int r, int g, int b)
            : this(r, g, b, 255)
        {
        }

        public Color(int r, int g, int b, int a)
        {
            this.r = ToHDR(r);
            this.g = ToHDR(g);
            this.b = ToHDR(b);
            this.a = ToHDR(a);
        }

        public Color(float r, float g, float b)
            : this(r, g, b, 1.0f)
        {
        }

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
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

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Color))
            {
                return false;
            }

            return this.Equals((Color)obj);
        }

        public override int GetHashCode()
        {
            var red = (int)LDR_R;
            var green = (int)LDR_G;
            var blue = (int)LDR_B;
            var alpha = (int)LDR_A;

            return (red << 24) | (green << 16) | (blue << 8) | alpha;
        }

        public override string ToString()
        {
            return "{" + String.Format("R:{0:f} G:{1:f} B:{2:f} A:{3:f}", R, G, B, A) + "}";
        }

        public Vector3 ToVector3()
        {
            return new Vector3(R, G, B);
        }

        public Vector4 ToVector4()
        {
            return new Vector4(R, G, B, A);
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
