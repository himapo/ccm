using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Rectangle
    {
        public int Height;

        public int Width;

        public int X;

        public int Y;

        public int Bottom { get { return Y + Height; } }

        public int Left { get { return X; } }

        public int Right { get { return X + Width; } }

        public int Top { get { return Y; } }

        public Point Center
        {
            get
            {
                return new Point((Left + Right) / 2, (Top + Bottom) / 2);
            }
        }

        public Rectangle(int x, int y, int width, int height)
        {
            Height = height;
            Width = width;
            X = x;
            Y = y;
        }
    }
}
