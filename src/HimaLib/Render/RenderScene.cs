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

    public class FontInfo
    {
        public Font Font { get; set; }
        public FontRenderParameter RenderParam { get; set; }
    }

    public class RenderScene
    {
        Dictionary<int, IRenderPath> PathDic = new Dictionary<int, IRenderPath>();

        public List<PointLight> PointLights { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public List<ModelInfo> ModelInfoList { get; set; }

        public List<BillboardInfo> BillboardInfoList { get; set; }

        public List<SphereInfo> SphereInfoList { get; set; }

        public List<CylinderInfo> CylinderInfoList { get; set; }

        public List<AABBInfo> AABBInfoList { get; set; }

        public List<FontInfo> FontInfoList { get; set; }

        public RenderScene()
        {
            PointLights = new List<PointLight>();
            DirectionalLights = new List<DirectionalLight>();
            ModelInfoList = new List<ModelInfo>();
            BillboardInfoList = new List<BillboardInfo>();
            SphereInfoList = new List<SphereInfo>();
            CylinderInfoList = new List<CylinderInfo>();
            AABBInfoList = new List<AABBInfo>();
            FontInfoList = new List<FontInfo>();
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

        public void Render()
        {
            foreach (var path in PathDic.Values)
            {
                path.PointLights = PointLights;
                path.DirectionalLights = DirectionalLights;
                path.ModelInfoList = ModelInfoList;
                path.BillboardInfoList = BillboardInfoList;
                path.SphereInfoList = SphereInfoList;
                path.CylinderInfoList = CylinderInfoList;
                path.AABBInfoList = AABBInfoList;
                path.FontInfoList = FontInfoList;
                path.Render();
            }
        }
    }
}
