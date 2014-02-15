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

        public Func<Vector3> Direction { get; set; }

        public Func<Vector3> Position { get; set; }

        public void Draw(ICollisionDrawer drawer, Color color)
        {
        }
    }
}
