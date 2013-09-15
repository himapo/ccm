using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class DepthModelRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.Depth; } }

        public AffineTransform Transform { get; set; }

        public DepthModelRenderParameter()
        {
            ShadowEnabled = false;
            IsTranslucent = false;
        }
    }
}
