using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class SimpleBillboardRenderParameter : IBillboardRenderParameter
    {
        public BillboardRendererType Type { get { return BillboardRendererType.Simple; } }

        public CameraBase Camera { get; set; }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public string TextureName { get; set; }
    }
}
