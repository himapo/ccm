using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class GBufferBillboardRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type { get { return BillboardRendererType.GBuffer; } }

        public ITexture Texture { get; set; }

        public GBufferBillboardRenderParameter()
        {
            IsShadowCaster = false;
            IsShadowReceiver = false;
            IsTranslucent = false;
            IsHud = false;
        }
    }
}
