using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class WireCollisionDrawer : ICollisionDrawer
    {
        public IRenderManager RenderManager { get; set; }

        public WireCollisionDrawer()
        {
            
        }

        public void DrawSphere(SphereCollisionPrimitive primitive, Color color)
        {
            var sphere = new SphereXna()
            {
                Position = primitive.Center(),
                Radius = primitive.Radius(),
            };

            var renderParam = new WireSphereRenderParameter()
            {
                Color = color,
            };

            RenderManager.RenderSphere(sphere, renderParam);
        }

        public void DrawCylinder(CylinderCollisionPrimitive primitive, Color color)
        {
            var cylinder = new CylinderXna()
            {
                Base = primitive.Base(),
                Radius = primitive.Radius(),
                Height = primitive.Height(),
            };

            var renderParam = new WireCylinderRenderParameter()
            {
                Color = color,
            };

            RenderManager.RenderCylinder(cylinder, renderParam);
        }

        public void DrawAABB(AABBCollisionPrimitive primitive, Color color)
        {
            var aabb = new AABBXna()
            {
                Corner = primitive.Corner,
                Width = primitive.Width,
            };

            var renderParam = new AABBRenderParameter()
            {
                Color = color,
            };

            RenderManager.RenderAABB(aabb, renderParam);
        }
    }
}
