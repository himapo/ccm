using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using HimaLib.Texture;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class ToneMappingRenderPath : RenderPath
    {
        public IBillboard Billboard { get; set; }

        public ITexture HDRScene { get; set; }

        public ToneMappingRenderPath()
        {
            ColorClearEnabled = true;
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

            var billboardInfoList = new List<BillboardInfo>();

            var renderParam = new ToneMappingRenderParameter();

            renderParam.HDRScene = HDRScene;

            billboardInfoList.Add(new BillboardInfo()
            {
                Billboard = this.Billboard,
                RenderParam = renderParam,
            });

            BillboardInfoList = billboardInfoList;

            base.Render();
        }
    }
}
