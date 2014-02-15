using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class RayCollisionPrimitive : ICollisionPrimitive
    {
        public CollisionShape Shape { get { return CollisionShape.Ray; } }

        public Ray Ray { get; set; }

        public void Draw(ICollisionDrawer drawer, Color color)
        {
        }
    }
}
