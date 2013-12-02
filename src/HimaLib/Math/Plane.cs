using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Plane : IEquatable<Plane>
    {
        public float D;

        public Vector3 Normal;

        public Plane(Vector3 normal, float d)
        {
            D = d;
            Normal = normal;
        }

        public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Normal = Vector3.Cross(point2 - point1, point3 - point1);

            D = -Vector3.Dot(Normal, point1) / Normal.Length();

            Normal.Normalize();
        }

        public static bool operator !=(Plane a, Plane b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return (a.D != b.D) || (a.Normal != b.Normal);
        }

        public static bool operator ==(Plane a, Plane b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return !(a != b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Plane))
            {
                return false;
            }

            return Equals((Plane)obj);
        }

        public bool Equals(Plane other)
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
