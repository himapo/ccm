using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Sphere : IEquatable<Sphere>
    {
        public Vector3 Center;

        public float Raduis;

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Raduis = radius;
        }

        public static bool operator !=(Sphere value1, Sphere value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return (value1.Center != value2.Center) || (value1.Raduis != value2.Raduis);
        }

        public static bool operator ==(Sphere value1, Sphere value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return !(value1 != value2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Sphere))
            {
                return false;
            }

            return Equals((Sphere)obj);
        }

        public bool Equals(Sphere other)
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
