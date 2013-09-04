using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class SimpleModelRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.Simple; } }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public SimpleModelRenderParameter()
        {
            Transform = new AffineTransform();
            Alpha = 1.0f;
            AmbientLightColor = Vector3.One * 0.4f;
        }
    }
}
