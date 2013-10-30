using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class AABBCollisionPrimitive : ICollisionPrimitive
    {
        public CollisionShape Shape { get { return CollisionShape.AABB; } }

        public Vector3 Corner { get; set; }

        public Vector3 Width { get; set; }

        public void Draw(ICollisionDrawer drawer, Color color)
        {
            drawer.DrawAABB(this, color);
        }
    }
}
