using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimaLib.System;
using HimaLib.Model;
using HimaLib.Light;

namespace HimaLib.Render
{
    public abstract class RenderManager : IRenderManager
    {
        RenderScene RenderScene = new RenderScene();

        public int BufferNum { get; private set; }

        int Buffer;

        List<PointLight>[] PointLights;

        List<DirectionalLight>[] DirectionalLights;

        List<ModelInfo>[] ModelInfoList;

        List<BillboardInfo>[] BillboardInfoList;

        List<SphereInfo>[] SphereInfoList;

        List<CylinderInfo>[] CylinderInfoList;

        List<AABBInfo>[] AABBInfoList;

        List<FontInfo>[] FontInfoList;

        public bool AsyncRender { get; set; }

        protected Task RenderTask;

        public RenderManager()
        {
            BufferNum = 2;

            PointLights = new List<PointLight>[BufferNum];
            DirectionalLights = new List<DirectionalLight>[BufferNum];
            ModelInfoList = new List<ModelInfo>[BufferNum];
            BillboardInfoList = new List<BillboardInfo>[BufferNum];
            SphereInfoList = new List<SphereInfo>[BufferNum];
            CylinderInfoList = new List<CylinderInfo>[BufferNum];
            AABBInfoList = new List<AABBInfo>[BufferNum];
            FontInfoList = new List<FontInfo>[BufferNum];

            for (var i = 0; i < BufferNum; ++i)
            {
                PointLights[i] = new List<PointLight>();
                DirectionalLights[i] = new List<DirectionalLight>();
                ModelInfoList[i] = new List<ModelInfo>();
                BillboardInfoList[i] = new List<BillboardInfo>();
                SphereInfoList[i] = new List<SphereInfo>();
                CylinderInfoList[i] = new List<CylinderInfo>();
                AABBInfoList[i] = new List<AABBInfo>();
                FontInfoList[i] = new List<FontInfo>();
            }

            AsyncRender = true;
        }

        public void AddPath(int index, IRenderPath path)
        {
            RenderScene.AddPath(index, path);
        }

        public IRenderPath GetPath(int index)
        {
            return RenderScene.GetPath(index);
        }

        public void RemovePath(int index)
        {
            RenderScene.RemovePath(index);
        }

        public void AddPointLight(PointLight light)
        {
            PointLights[Buffer].Add(light);
        }

        public void RemovePointLight(PointLight light)
        {
            PointLights[Buffer].Remove(light);
        }

        public void ClearPointLight()
        {
            PointLights[Buffer].Clear();
        }

        public void AddDirectionalLight(DirectionalLight light)
        {
            DirectionalLights[Buffer].Add(light);
        }

        public void RemoveDirectionalLight(DirectionalLight light)
        {
            DirectionalLights[Buffer].Remove(light);
        }

        public void ClearDirectionalLight()
        {
            DirectionalLights[Buffer].Clear();
        }

        public void RenderModel(IModel primitive, ModelRenderParameter renderParam)
        {
            ModelInfoList[Buffer].Add(new ModelInfo() { Model = primitive, RenderParam = renderParam });
        }

        public void RenderBillboard(IBillboard primitive, BillboardRenderParameter renderParam)
        {
            BillboardInfoList[Buffer].Add(new BillboardInfo() { Billboard = primitive, RenderParam = renderParam });
        }

        public void RenderSphere(Sphere primitive, SphereRenderParameter renderParam)
        {
            SphereInfoList[Buffer].Add(new SphereInfo() { Sphere = primitive, RenderParam = renderParam });
        }

        public void RenderCylinder(Cylinder primitive, CylinderRenderParameter renderParam)
        {
            CylinderInfoList[Buffer].Add(new CylinderInfo() { Cylinder = primitive, RenderParam = renderParam });
        }

        public void RenderAABB(AABB primitive, AABBRenderParameter renderParam)
        {
            AABBInfoList[Buffer].Add(new AABBInfo() { AABB = primitive, RenderParam = renderParam });
        }

        public void RenderFont(Font primitive, FontRenderParameter renderParam)
        {
            FontInfoList[Buffer].Add(new FontInfo() { Font = primitive, RenderParam = renderParam });
        }

        protected void Render()
        {
            var prev = GetPrevBuffer();

            RenderScene.PointLights = PointLights[prev];
            RenderScene.DirectionalLights = DirectionalLights[prev];
            RenderScene.ModelInfoList = ModelInfoList[prev];
            RenderScene.BillboardInfoList = BillboardInfoList[prev];
            RenderScene.SphereInfoList = SphereInfoList[prev];
            RenderScene.CylinderInfoList = CylinderInfoList[prev];
            RenderScene.AABBInfoList = AABBInfoList[prev];
            RenderScene.FontInfoList = FontInfoList[prev];

            RenderScene.Render();
        }

        public abstract void StartRender();

        public abstract void WaitRender();

        protected void IncrementBuffer()
        {
            if (++Buffer == BufferNum)
            {
                Buffer = 0;
            }
        }

        int GetPrevBuffer()
        {
            return (Buffer == 0) ? (BufferNum - 1) : (Buffer - 1);
        }

        protected void CopyPrevBuffer()
        {
            var prev = GetPrevBuffer();

            PointLights[Buffer].Clear();
            PointLights[Buffer].AddRange(PointLights[prev]);

            DirectionalLights[Buffer].Clear();
            DirectionalLights[Buffer].AddRange(DirectionalLights[prev]);
        }

        protected void ClearBuffer()
        {
            ModelInfoList[Buffer].Clear();
            BillboardInfoList[Buffer].Clear();
            SphereInfoList[Buffer].Clear();
            CylinderInfoList[Buffer].Clear();
            AABBInfoList[Buffer].Clear();
            FontInfoList[Buffer].Clear();
        }
    }
}
