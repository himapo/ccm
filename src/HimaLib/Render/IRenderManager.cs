using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface IRenderManager
    {
        void RenderModel(IModel primitive, ModelRenderParameter renderParam);

        void RenderBillboard(IBillboard primitive, BillboardRenderParameter renderParam);

        void RenderSphere(Sphere primitive, SphereRenderParameter renderParam);

        void RenderCylinder(Cylinder primitive, CylinderRenderParameter renderParam);

        void RenderAABB(AABB primitive, AABBRenderParameter renderParam);

        void RenderFont(Font primitive, FontRenderParameter renderParam);
    }
}
