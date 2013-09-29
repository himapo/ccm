using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using HimaLib.Texture;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class LightBufferRenderPath : RenderPath
    {
        public IModel SphereModel { get; set; }

        public IBillboard Billboard { get; set; }

        public ITexture NormalDepthMap { get; set; }

        public LightBufferRenderPath()
        {
            ClearEnabled = true;
            ClearColor = Color.Gray;

            DepthSortEnabled = true;
            DepthTestEnabled = false;
            DepthWriteEnabled = false;
            DepthClearEnabled = false;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = false;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = false;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = false;
            RenderHudBillboardOnly = true;
        }

        public override void Render()
        {
            BillboardInfoList = new List<BillboardInfo>();
            ModelInfoList = new List<ModelInfo>();

            ClearEnabled = true;
            DepthTestEnabled = false;
            RenderModelEnabled = false;
            RenderBillboardEnabled = true;

            RenderDirectionalLights();

            ClearEnabled = false;
            DepthTestEnabled = true;
            RenderModelEnabled = true;
            RenderBillboardEnabled = false;

            RenderPointLights();
        }

        void RenderPointLights()
        {
            var modelInfoList = new List<ModelInfo>();

            foreach (var light in PointLights)
            {
                var transform = new AffineTransform()
                {
                    Scale = Vector3.One * light.AttenuationEnd,
                    Translation = light.Position,
                };

                var renderParam = new PointLightRenderParameter()
                {
                    Transform = transform,
                    PointLight = light,
                    NormalDepthMap = NormalDepthMap,
                };

                modelInfoList.Add(new ModelInfo()
                {
                    Model = SphereModel,
                    RenderParam = renderParam,
                });
            }

            ModelInfoList = modelInfoList;

            base.Render();
        }

        void RenderDirectionalLights()
        {
            foreach (var light in DirectionalLights)
            {
                var renderParam = new DirectionalLightRenderParameter()
                {
                    DirectionalLight = light,
                    NormalDepthMap = NormalDepthMap,
                };

                BillboardInfoList.Add(new BillboardInfo()
                {
                    Billboard = this.Billboard,
                    RenderParam = renderParam,
                });
            }

            base.Render();
        }
    }
}
