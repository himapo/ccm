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

        public DepthModelRenderParameter()
        {
            IsShadowCaster = false;
            IsTranslucent = false;
        }
    }
}
