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
    public class SimpleBillboardRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type { get { return BillboardRendererType.Simple; } }

        public float Alpha { get; set; }

        public ITexture Texture { get; set; }

        public AffineTransform AffineTransform { get; set; }

        public SimpleBillboardRenderParameter()
        {
            IsHud = false;
        }
    }
}
