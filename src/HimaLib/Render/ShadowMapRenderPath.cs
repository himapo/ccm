using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class ShadowMapRenderPath : RenderPath
    {
        public ShadowMapRenderPath()
        {
            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = true;

            RenderModelEnabled = true;
            RenderShadowModelOnly = true;
            RenderOpaqueModelOnly = true;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = true;
            RenderOpaqueBillboardOnly = true;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = true;
            RenderHudBillboardOnly = false;
        }

        public override void Render()
        {
            foreach (var info in ModelInfoList)
            {
                if (!info.RenderParam.ShadowEnabled)
                {
                    continue;
                }

                if (info.RenderParam.IsTranslucent)
                {
                    continue;
                }

                // TODO : 深度レンダラに差し替え
            }

            base.Render();
        }
    }
}
