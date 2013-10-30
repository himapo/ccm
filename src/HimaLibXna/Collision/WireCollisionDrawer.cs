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
        public RenderScene RenderScene { get; set; }

        WireSphereRenderParameter SphereRenderParam = new WireSphereRenderParameter();

        WireCylinderRenderParameter CylinderRenderParam = new WireCylinderRenderParameter();

        AABBRenderParameter AABBRenderParam = new AABBRenderParameter();

        public WireCollisionDrawer()
        {
            AABBRenderParam.Type = AABBRendererType.Wire;
        }

        public void DrawSphere(SphereCollisionPrimitive primitive, Color color)
        {
            var sphere = new SphereXna()
            {
                Position = primitive.Center(),
                Radius = primitive.Radius(),
            };

            SphereRenderParam.Color = color;

            RenderScene.RenderSphere(sphere, SphereRenderParam);
        }

        public void DrawCylinder(CylinderCollisionPrimitive primitive, Color color)
        {
            var cylinder = new CylinderXna()
            {
                Base = primitive.Base(),
                Radius = primitive.Radius(),
                Height = primitive.Height(),
            };

            CylinderRenderParam.Color = color;

            RenderScene.RenderCylinder(cylinder, CylinderRenderParam);
        }

        public void DrawAABB(AABBCollisionPrimitive primitive, Color color)
        {
            var aabb = new AABBXna()
            {
                Corner = primitive.Corner,
                Width = primitive.Width,
            };

            AABBRenderParam.Color = color;

            RenderScene.RenderAABB(aabb, AABBRenderParam);
        }
    }
}
