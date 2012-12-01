using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public struct Point : IEquatable<Point>
    {
        Microsoft.Xna.Framework.Point XnaPoint;

        public int X { get { return XnaPoint.X; } set { XnaPoint.X = value; } }

        public int Y { get { return XnaPoint.Y; } set { XnaPoint.Y = value; } }

        public static Point Zero { get { return new Point(Microsoft.Xna.Framework.Point.Zero); } }

        public Point(int x, int y)
        {
            XnaPoint = new Microsoft.Xna.Framework.Point(x, y);
        }

        public Point(Microsoft.Xna.Framework.Point xnaPoint)
        {
            XnaPoint = xnaPoint;
        }

        public static bool operator !=(Point a, Point b)
        {
            return (a.XnaPoint != b.XnaPoint);
        }

        public static bool operator ==(Point a, Point b)
        {
            return (a.XnaPoint == b.XnaPoint);
        }

        public override bool Equals(object obj)
        {
            return XnaPoint.Equals(obj);
        }

        public bool Equals(Point other)
        {
            return XnaPoint.Equals(other.XnaPoint);
        }

        public override int GetHashCode()
        {
            return XnaPoint.GetHashCode();
        }

        public override string ToString()
        {
            return XnaPoint.ToString();
        }
    }
}
