using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public interface ICollisionDrawer
    {
        RenderScene RenderScene { get;  set; }

        void DrawSphere(SphereCollisionPrimitive primitive, Color color);

        void DrawCylinder(CylinderCollisionPrimitive primitive, Color color);

        void DrawAABB(AABBCollisionPrimitive primitive, Color color);
    }
}
