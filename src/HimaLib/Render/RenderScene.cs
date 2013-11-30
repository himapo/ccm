using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class ModelInfo
    {
        public IModel Model { get; set; }
        public ModelRenderParameter RenderParam { get; set; }
    }

    public class BillboardInfo
    {
        public IBillboard Billboard { get; set; }
        public BillboardRenderParameter RenderParam { get; set; }
    }

    public class SphereInfo
    {
        public Sphere Sphere { get; set; }
        public SphereRenderParameter RenderParam { get; set; }
    }

    public class CylinderInfo
    {
        public Cylinder Cylinder { get; set; }
        public CylinderRenderParameter RenderParam { get; set; }
    }

    public class AABBInfo
    {
        public AABB AABB { get; set; }
        public AABBRenderParameter RenderParam { get; set; }
    }

    public class RenderScene
    {
        Dictionary<int, IRenderPath> PathDic = new Dictionary<int, IRenderPath>();

        List<PointLight> PointLights = new List<PointLight>();

        List<DirectionalLight> DirectionalLights = new List<DirectionalLight>();

        List<ModelInfo> ModelInfoList = new List<ModelInfo>();

        List<BillboardInfo> BillboardInfoList = new List<BillboardInfo>();

        List<SphereInfo> SphereInfoList = new List<SphereInfo>();

        List<CylinderInfo> CylinderInfoList = new List<CylinderInfo>();

        List<AABBInfo> AABBInfoList = new List<AABBInfo>();

        public RenderScene()
        {
        }

        public void AddPath(int index, IRenderPath path)
        {
            PathDic[index] = path;
        }

        public IRenderPath GetPath(int index)
        {
            return PathDic[index];
        }

        public void RemovePath(int index)
        {
            PathDic.Remove(index);
        }

        public void AddPointLight(PointLight light)
        {
            if (!PointLights.Contains(light))
            {
                PointLights.Add(light);
            }
        }

        public void RemovePointLight(PointLight light)
        {
            PointLights.Remove(light);
        }

        public void ClearPointLight()
        {
            PointLights.Clear();
        }

        public void AddDirectionalLight(DirectionalLight light)
        {
            if (!DirectionalLights.Contains(light))
            {
                DirectionalLights.Add(light);
            }
        }

        public void RemoveDirectionalLight(DirectionalLight light)
        {
            DirectionalLights.Remove(light);
        }

        public void ClearDirectionalLight()
        {
            DirectionalLights.Clear();
        }

        public void RenderModel(IModel model, ModelRenderParameter renderParam)
        {
            ModelInfoList.Add(new ModelInfo() { Model = model, RenderParam = renderParam });
        }

        public void RenderBillboard(IBillboard billboard, BillboardRenderParameter renderParam)
        {
            BillboardInfoList.Add(new BillboardInfo() { Billboard = billboard, RenderParam = renderParam });
        }

        public void RenderSphere(Sphere sphere, SphereRenderParameter renderParam)
        {
            SphereInfoList.Add(new SphereInfo() { Sphere = sphere, RenderParam = renderParam });
        }

        public void RenderCylinder(Cylinder cylinder, CylinderRenderParameter renderParam)
        {
            CylinderInfoList.Add(new CylinderInfo() { Cylinder = cylinder, RenderParam = renderParam });
        }

        public void RenderAABB(AABB aabb, AABBRenderParameter renderParam)
        {
            AABBInfoList.Add(new AABBInfo() { AABB = aabb, RenderParam = renderParam });
        }

        public void Render()
        {
            // TODO : 別スレッド化
            InnerRender();
        }

        void InnerRender()
        {
            FrameCacheDataBase.Instance.Clear();

            foreach (var path in PathDic.Values)
            {
                path.PointLights = PointLights;
                path.DirectionalLights = DirectionalLights;
                path.ModelInfoList = ModelInfoList;
                path.BillboardInfoList = BillboardInfoList;
                path.SphereInfoList = SphereInfoList;
                path.CylinderInfoList = CylinderInfoList;
                path.AABBInfoList = AABBInfoList;
                path.Render();
            }

            ModelInfoList.Clear();
            BillboardInfoList.Clear();
            SphereInfoList.Clear();
            CylinderInfoList.Clear();
            AABBInfoList.Clear();
        }
    }
}
