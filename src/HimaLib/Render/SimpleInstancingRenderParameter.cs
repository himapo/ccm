using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class SimpleInstancingRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.SimpleInstancing; } }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get; set; }

        public bool IsTranslucent { get; set; }

        public bool TransformsUpdated { get; set; }

        public List<AffineTransform> Transforms { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0SpecularColor { get; set; }

        public SimpleInstancingRenderParameter()
        {
            ShadowEnabled = true;
            IsTranslucent = false;
            TransformsUpdated = false;
        }
    }
}
