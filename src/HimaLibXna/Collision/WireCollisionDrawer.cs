using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using HimaLib.Render;

namespace HimaLib.Collision
{
    public class WireCollisionDrawer : ICollisionDrawer
    {
        public RenderScene RenderScene { get; set; }

        WireSphereRenderParameter SphereRenderParam = new WireSphereRenderParameter();

        WireCylinderRenderParameter CylinderRenderParam = new WireCylinderRenderParameter();

        AABBRenderParameter AABBRenderParam = new AABBRenderParameter();

        public WireCollisionDrawer()
        {
            AABBRenderParam.Type = AABBRendererType.Wire;
        }

        public void DrawSphere(SphereCollisionPrimitive primitive, bool active)
        {
            var sphere = new SphereXna()
            {
                Position = primitive.Center(),
                Radius = primitive.Radius(),
            };

            RenderScene.RenderSphere(sphere, SphereRenderParam);
        }

        public void DrawCylinder(CylinderCollisionPrimitive primitive, bool active)
        {
            var cylinder = new CylinderXna()
            {
                Base = primitive.Base(),
                Radius = primitive.Radius(),
                Height = primitive.Height(),
            };

            RenderScene.RenderCylinder(cylinder, CylinderRenderParam);
        }

        public void DrawAABB(AABBCollisionPrimitive primitive, bool active)
        {
            var aabb = new AABBXna()
            {
                Corner = primitive.Corner,
                Width = primitive.Width,
            };

            RenderScene.RenderAABB(aabb, AABBRenderParam);
        }
    }
}
