using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public struct Ray : IEquatable<Ray>
    {
        public Vector3 Direction;

        public Vector3 Position;

        public Ray(Vector3 direction, Vector3 position)
        {
            Direction = direction;
            Position = position;
        }

        public static bool operator !=(Ray value1, Ray value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return (value1.Direction != value2.Direction) || (value1.Position != value2.Position);
        }

        public static bool operator ==(Ray value1, Ray value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return !(value1 != value2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Ray))
            {
                return false;
            }

            return Equals((Ray)obj);
        }

        public bool Equals(Ray other)
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

        public Nullable<float> Intersects(Sphere sphere)
        {
            var toSphere = sphere.Center - Position;

            // 球の中心とレイの距離を求める
            // 外積の絶対値は二辺のなす平行四辺形の面積を表す
            // 底辺Directionは単位ベクトルなので面積は垂線の長さと等しい
            var distance = Vector3.Cross(toSphere, Direction).Length();

            if (distance >= sphere.Raduis)
            {
                return null;
            }

            // Positionから「円の中心から直線に下ろした垂線の足H」までの距離
            var lengthPH = Vector3.Dot(toSphere, Direction);

            // Positionから交点までの距離
            var lengthIntersect = lengthPH - MathUtil.Sqrt(
                sphere.Raduis * sphere.Raduis - distance * distance);

            return lengthIntersect;
        }
    }
}
