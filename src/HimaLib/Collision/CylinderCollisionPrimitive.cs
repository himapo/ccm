using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class CylinderCollisionPrimitive : ICollisionPrimitive
    {
        public CollisionShape Shape { get { return CollisionShape.Cylinder; } }

        public Func<Vector3> Base { get; set; }

        public Func<float> Radius { get; set; }

        public Func<float> Height { get; set; }

        public void Draw(ICollisionDrawer drawer, Color color)
        {
            drawer.DrawCylinder(this, color);
        }
    }
}
