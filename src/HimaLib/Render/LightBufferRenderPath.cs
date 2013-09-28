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

            RenderModelEnabled = false;
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
            ModelInfoList = new List<ModelInfo>();

            BillboardInfoList = new List<BillboardInfo>();

            foreach (var light in DirectionalLights)
            {
                var renderParam = new DirectionalLightRenderParameter();

                renderParam.DirectionalLight = light;
                renderParam.NormalDepthMap = NormalDepthMap;

                BillboardInfoList.Add(new BillboardInfo()
                {
                    Billboard = this.Billboard,
                    RenderParam = renderParam,
                });
            }

            // TODO : PointLight

            base.Render();
        }
    }
}
