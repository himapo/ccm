using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        void Draw(ICollisionDrawer drawer, bool active);
    }
}
