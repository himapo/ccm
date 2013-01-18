using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public interface ICollisionDrawer
    {
        void DrawSphere(SphereCollisionPrimitive primitive, bool active);

        void DrawCylinder(CylinderCollisionPrimitive primitive, bool active);
    }
}
