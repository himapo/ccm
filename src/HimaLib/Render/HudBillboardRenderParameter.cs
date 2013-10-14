using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class HudBillboardRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type { get { return BillboardRendererType.Hud; } }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public ITexture Texture { get; set; }

        public Vector2 RectOffset { get; set; }

        public Vector2 RectSize { get; set; }

        public HudBillboardRenderParameter()
        {
            IsShadowCaster = false;
            IsShadowReceiver = false;
            IsHud = true;
        }
    }
}
