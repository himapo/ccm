using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimaLib.Render;
using HimaLib.System;
using HimaLib.Model;
using HimaLib.Light;

namespace ccm.Render
{
    public class RenderManager : IRenderManager
    {
        public static RenderManager Instance
        {
            get
            {
                return Singleton<RenderManager>.Instance;
            }
        }

        static readonly int BUFFER_NUM = 2;

        int Buffer;

        RenderScene RenderScene { get; set; }

        Task RenderTask;

        List<PointLight>[] PointLights = new List<PointLight>[BUFFER_NUM];

        List<DirectionalLight>[] DirectionalLights = new List<DirectionalLight>[BUFFER_NUM];

        List<ModelInfo>[] ModelInfoList = new List<ModelInfo>[BUFFER_NUM];

        List<BillboardInfo>[] BillboardInfoList = new List<BillboardInfo>[BUFFER_NUM];

        List<SphereInfo>[] SphereInfoList = new List<SphereInfo>[BUFFER_NUM];

        List<CylinderInfo>[] CylinderInfoList = new List<CylinderInfo>[BUFFER_NUM];

        List<AABBInfo>[] AABBInfoList = new List<AABBInfo>[BUFFER_NUM];

        List<FontInfo>[] FontInfoList = new List<FontInfo>[BUFFER_NUM];

        RenderManager()
        {
            RenderScene = new RenderScene();

            for (var i = 0; i < BUFFER_NUM; ++i)
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
        }

        public void AddPath(RenderPathType index, IRenderPath path)
        {
            RenderScene.AddPath((int)index, path);
        }

        public IRenderPath GetPath(RenderPathType index)
        {
            return RenderScene.GetPath((int)index);
        }

        public void RemovePath(RenderPathType index)
        {
            RenderScene.RemovePath((int)index);
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

        void Render()
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

        public void StartRender()
        {
            IncrementBuffer();

            CopyPrevBuffer();

            ClearBuffer();

            RenderTask = Task.Factory.StartNew(Render);
        }

        public void WaitRender()
        {
            RenderTask.Wait();
        }

        void IncrementBuffer()
        {
            if (++Buffer == BUFFER_NUM)
            {
                Buffer = 0;
            }
        }

        int GetPrevBuffer()
        {
            return (Buffer == 0) ? (BUFFER_NUM - 1) : (Buffer - 1);
        }

        void CopyPrevBuffer()
        {
            var prev = GetPrevBuffer();

            PointLights[Buffer].Clear();
            PointLights[Buffer].AddRange(PointLights[prev]);

            DirectionalLights[Buffer].Clear();
            DirectionalLights[Buffer].AddRange(DirectionalLights[prev]);
        }

        void ClearBuffer()
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
