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

        public ToneMappingRenderParameter RenderParam { get; set; }

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
            if (!Enabled)
            {
                return;
            }

            //RenderDevice.SetRenderTarget(RenderTargetIndex);

            //ClearTarget();

            RenderParam.RenderDevice = RenderDevice;

            var billboardInfoList = new List<BillboardInfo>();

            billboardInfoList.Add(new BillboardInfo()
            {
                Billboard = this.Billboard,
                RenderParam = this.RenderParam,
            });

            BillboardInfoList = billboardInfoList;

            RenderBillboard();
        }

        void ClearTarget()
        {
            if (ColorClearEnabled && DepthClearEnabled)
            {
                RenderDevice.ClearAll(ClearColor);
            }
            else if (ColorClearEnabled)
            {
                RenderDevice.ClearColor(ClearColor);
            }
            else if (DepthClearEnabled)
            {
                RenderDevice.ClearDepth();
            }
        }
    }
}
