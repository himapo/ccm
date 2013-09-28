using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class GBufferModelRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.GBuffer; } }

        public AffineTransform Transform { get; set; }

        public ITexture Texture { get; set; }

        public GBufferModelRenderParameter()
        {
            IsShadowCaster = false;
            IsTranslucent = false;
        }
    }
}
