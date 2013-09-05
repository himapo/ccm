using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class TranslucentRenderPath : RenderPath
    {
        public TranslucentRenderPath()
        {
            DepthSortEnabled = true;
            DepthTestEnabled = true;
            DepthWriteEnabled = false;
            DepthClearEnabled = false;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = false;
            RenderTranslucentModelOnly = true;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = false;
            RenderTranslucentBillboardOnly = true;
            RenderNoHudBillboardOnly = true;
            RenderHudBillboardOnly = false;
        }
    }
}
