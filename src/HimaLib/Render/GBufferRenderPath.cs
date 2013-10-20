using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class GBufferRenderPath : RenderPath
    {
        public GBufferRenderPath()
        {
            ColorClearEnabled = true;
            ClearColor = Color.Gray;

            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = true;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = true;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = true;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = true;
            RenderHudBillboardOnly = false;
        }

        public override void Render()
        {
            CreateModelList();
            CreateBillboardList();

            base.Render();
        }

        void CreateModelList()
        {
            ModelInfoList = ModelInfoList.Where(
            info =>
            {
                return info.RenderParam.GBufferEnabled;
            }).Select(
            info =>
            {
                // Gバッファ用のModelInfoを新たに生成する
                var gbufferParam = new GBufferModelRenderParameter()
                {
                    Transform = info.RenderParam.Transform,
                    ModelType = info.RenderParam.ModelType,
                    TransformsUpdated = info.RenderParam.TransformsUpdated,
                    InstanceTransforms = info.RenderParam.InstanceTransforms,
                };

                return new ModelInfo()
                {
                    Model = info.Model,
                    RenderParam = gbufferParam,
                };
            });
        }

        void CreateBillboardList()
        {
            BillboardInfoList = BillboardInfoList.Where(
            info =>
            {
                return info.RenderParam.GBufferEnabled;
            }).Select(
            info =>
            {
                // Gバッファ用のBillboardInfoを新たに生成する
                var gbufferParam = new GBufferBillboardRenderParameter()
                {
                    Transform = info.RenderParam.Transform,
                };

                return new BillboardInfo()
                {
                    Billboard = info.Billboard,
                    RenderParam = gbufferParam,
                };
            });
        }
    }
}
