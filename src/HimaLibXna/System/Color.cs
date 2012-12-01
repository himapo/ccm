using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.System
{
    public struct Color : IColor
    {
        Microsoft.Xna.Framework.Color XnaColor;

        public byte R { get { return XnaColor.R; } set { XnaColor.R = value; } }

        public byte G { get { return XnaColor.G; } set { XnaColor.G = value; } }

        public byte B { get { return XnaColor.B; } set { XnaColor.B = value; } }

        public byte A { get { return XnaColor.A; } set { XnaColor.A = value; } }

        public static Microsoft.Xna.Framework.Color CreateXnaColor(IColor color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
        }

        public Color(IColor color)
        {
            XnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
        }

        public Color(Microsoft.Xna.Framework.Color XnaColor)
        {
            this.XnaColor = XnaColor;
        }

        public Color(float r, float g, float b)
        {
            XnaColor = new Microsoft.Xna.Framework.Color(r, g, b);
        }

        public Color(int r, int g, int b)
        {
            XnaColor = new Microsoft.Xna.Framework.Color(r, g, b);
        }

        public Color(float r, float g, float b, float a)
        {
            XnaColor = new Microsoft.Xna.Framework.Color(r, g, b, a);
        }

        public Color(int r, int g, int b, int a)
        {
            XnaColor = new Microsoft.Xna.Framework.Color(r, g, b, a);
        }

        public override string ToString()
        {
            return XnaColor.ToString();
        }

        public static Color Black { get { return new Color(Microsoft.Xna.Framework.Color.Black); } }

        public static Color Blue { get { return new Color(Microsoft.Xna.Framework.Color.Blue); } }
        
        public static Color Green { get { return new Color(Microsoft.Xna.Framework.Color.Green); } }

        public static Color Gray { get { return new Color(Microsoft.Xna.Framework.Color.Gray); } }

        public static Color LightBlue { get { return new Color(Microsoft.Xna.Framework.Color.LightBlue); } }

        public static Color LightGreen { get { return new Color(Microsoft.Xna.Framework.Color.LightGreen); } }

        public static Color Purple { get { return new Color(Microsoft.Xna.Framework.Color.Purple); } }
        
        public static Color Red { get { return new Color(Microsoft.Xna.Framework.Color.Red); } }

        public static Color White { get { return new Color(Microsoft.Xna.Framework.Color.White); } }
        
        public static Color Yellow { get { return new Color(Microsoft.Xna.Framework.Color.Yellow); } }
    }
}
