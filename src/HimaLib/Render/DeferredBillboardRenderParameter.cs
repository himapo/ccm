using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Texture;
using HimaLib.Math;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class DeferredBillboardRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type { get { return BillboardRendererType.Deferred; } }

        public ITexture AlbedoMap { get; set; }

        public ITexture PositionMap { get; set; }

        public ITexture NormalDepthMap { get; set; }

        public DirectionalLight DirectionalLight { get; set; }

        public DeferredBillboardRenderParameter()
        {
            IsHud = true;
        }
    }
}
