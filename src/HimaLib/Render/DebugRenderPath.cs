using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class DebugRenderPath : RenderPath
    {
        public DebugRenderPath()
        {
            Enabled = true;

            ColorClearEnabled = false;

            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = false;

            RenderModelEnabled = false;
            RenderBillboardEnabled = false;

            RenderSphereEnabled = true;
            RenderCylinderEnabled = true;
            RenderAABBEnabled = true;
        }
    }
}
