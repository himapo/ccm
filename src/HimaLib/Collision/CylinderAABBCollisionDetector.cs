using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class CylinderAABBCollisionDetector : ICollisionDetector
    {
        public CylinderCollisionPrimitive Cylinder { get; set; }

        public AABBCollisionPrimitive AABB { get; set; }

        public bool Detect()
        {
            // 垂直交差判定
            var al = Cylinder.Base().Y;
            var ah = al + Cylinder.Height();
            var bl = AABB.Corner.Y;
            var bh = bl + AABB.Width.Y;
            var vertical =
                (al > bl && al < bh)
                || (ah > bl && ah < bh)
                || (bl > al && bl < ah)
                || (bh > al && bh < ah);

            if(!vertical)
            {
                return false;
            }

            // 水平交差判定

            if (Cylinder.Base().X + Cylinder.Radius() < AABB.Corner.X
                || Cylinder.Base().X - Cylinder.Radius() > AABB.Corner.X + AABB.Width.X)
            {
                return false;
            }

            if (Cylinder.Base().Z + Cylinder.Radius() < AABB.Corner.Z
                || Cylinder.Base().Z - Cylinder.Radius() > AABB.Corner.Z + AABB.Width.Z)
            {
                return false;
            }

            if(Cylinder.Base().X > AABB.Corner.X
                && Cylinder.Base().X < AABB.Corner.X + AABB.Width.X)
            {
                return true;
            }

            if (Cylinder.Base().Z > AABB.Corner.Z
                && Cylinder.Base().Z < AABB.Corner.Z + AABB.Width.Z)
            {
                return true;
            }

            // 4つの角のうち最短距離が筒の半径以内か

            var corners = new List<Vector2>();

            corners.Add(new Vector2(AABB.Corner.X, AABB.Corner.Z));
            corners.Add(new Vector2(AABB.Corner.X + AABB.Width.X, AABB.Corner.Z));
            corners.Add(new Vector2(AABB.Corner.X + AABB.Width.X, AABB.Corner.Z + AABB.Width.Z));
            corners.Add(new Vector2(AABB.Corner.X, AABB.Corner.Z + AABB.Width.Z));

            var cylinderCenter = new Vector2(Cylinder.Base().X, Cylinder.Base().Z);

            var minLength = corners.Min((corner) =>
            {
                return (corner - cylinderCenter).LengthSquared();
            });

            return minLength < (Cylinder.Radius() * Cylinder.Radius());
        }
    }
}
