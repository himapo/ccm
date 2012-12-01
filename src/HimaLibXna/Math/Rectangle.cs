using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public struct Rectangle
    {
        Microsoft.Xna.Framework.Rectangle XnaRectangle;

        public int Height { get { return XnaRectangle.Height; } set { XnaRectangle.Height = value; } }

        public int Width { get { return XnaRectangle.Width; } set { XnaRectangle.Width = value; } }

        public int X { get { return XnaRectangle.X; } set { XnaRectangle.X = value; } }

        public int Y { get { return XnaRectangle.Y; } set { XnaRectangle.Y = value; } }

        public int Bottom { get { return XnaRectangle.Bottom; } }

        public int Left { get { return XnaRectangle.Left; } }

        public int Right { get { return XnaRectangle.Right; } }

        public int Top { get { return XnaRectangle.Top; } }

        public Rectangle(int x, int y, int width, int height)
        {
            XnaRectangle = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
        }

        public Rectangle(Microsoft.Xna.Framework.Rectangle xnaRectangle)
        {
            XnaRectangle = xnaRectangle;
        }
    }
}
