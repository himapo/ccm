using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class DepthModelRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.Depth; } }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get { return false; } set { } }

        public bool IsTranslucent { get { return false; } set { } }

        public AffineTransform Transform { get; set; }

        public DepthModelRenderParameter()
        {
        }
    }
}
