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
            ColorClearEnabled = true;
            ClearColor = Color.Black;

            DepthSortEnabled = true;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
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
            RenderDirectionalLights();

            RenderPointLights();

            base.Render();
        }

        void RenderPointLights()
        {
            var modelInfoList = new List<ModelInfo>();

            var query = PointLights.Select((light, index) => new { light, index });

            foreach (var v in query)
            {
                var transform = new AffineTransform()
                {
                    Scale = Vector3.One * v.light.AttenuationEnd,
                    Translation = v.light.Position,
                };

                var renderParam = new PointLightRenderParameter()
                {
                    Transform = transform,
                    PointLight = v.light,
                    LightID = v.index + 1,
                    NormalDepthMap = NormalDepthMap,
                };

                modelInfoList.Add(new ModelInfo()
                {
                    Model = SphereModel,
                    RenderParam = renderParam,
                });
            }

            ModelInfoList = modelInfoList;
        }

        void RenderDirectionalLights()
        {
            BillboardInfoList = new List<BillboardInfo>();            

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
        }
    }
}
