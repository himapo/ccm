using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public enum CollisionShape
    {
        Sphere,
        Cylinder,
        AABB,
    }

    public interface ICollisionPrimitive
    {
        CollisionShape Shape { get; }

        void Draw(ICollisionDrawer drawer, Color color);
    }
}
