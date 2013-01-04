using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Point : IEquatable<Point>
    {
        public int X;

        public int Y;

        public static Point Zero { get { return new Point(0, 0); } }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return (a.X != b.X) || (a.Y != b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return !(a != b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);

            if (!(obj is Rectangle))
            {
                throw new InvalidCastException("The 'obj' argument is not a Rectangle object.");
            }
            else
            {
                return Equals((Rectangle)obj);
            }
        }

        public bool Equals(Point other)
        {
            return (this == other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
