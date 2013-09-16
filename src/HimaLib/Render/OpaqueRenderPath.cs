using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class OpaqueRenderPath : RenderPath
    {
        public OpaqueRenderPath()
        {
            ClearEnabled = true;
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
    }
}
