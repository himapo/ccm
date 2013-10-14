using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Collision
{
    public interface ICollisionDrawer
    {
        RenderScene RenderScene { get;  set; }

        void DrawSphere(SphereCollisionPrimitive primitive, bool active);

        void DrawCylinder(CylinderCollisionPrimitive primitive, bool active);
    }
}
