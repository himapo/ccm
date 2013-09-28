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
    public class SimpleModelRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.Simple; } }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public SimpleModelRenderParameter()
        {
            Alpha = 1.0f;
            AmbientLightColor = Vector3.One * 0.4f;
        }
    }
}
