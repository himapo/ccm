using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class ToonModelRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.Toon; } }

        public AffineTransform Transform { get; set; }

        public ToonModelRenderParameter()
        {
            IsShadowCaster = true;
            IsTranslucent = false;
            Transform = new AffineTransform();
        }
    }
}
