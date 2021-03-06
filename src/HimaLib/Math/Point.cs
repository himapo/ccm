﻿using System;
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
            if (a == null || b == null)
            {
                return false;
            }

            return (a.X != b.X) || (a.Y != b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return !(a != b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
            {
                return false;
            }

            return Equals((Point)obj);
        }

        public bool Equals(Point other)
        {
            if (other == null)
            {
                return false;
            }

            return (this == other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
