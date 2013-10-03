using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Texture;
using HimaLib.Math;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class PointLightRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type
        {
            get { return ModelRendererType.PointLight; }
        }

        public ITexture NormalDepthMap { get; set; }

        public PointLight PointLight { get; set; }

        public int LightID { get; set; }

        public PointLightRenderParameter()
        {
        }
    }
}
