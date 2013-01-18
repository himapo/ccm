using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class SphereCollisionPrimitive : ICollisionPrimitive
    {
        public CollisionShape Shape { get { return CollisionShape.Sphere; } }

        public Func<Vector3> Center { get; set; }

        public Func<float> Radius { get; set; }

        public void Draw(ICollisionDrawer drawer, bool active)
        {
            drawer.DrawSphere(this, active);
        }
    }
}
