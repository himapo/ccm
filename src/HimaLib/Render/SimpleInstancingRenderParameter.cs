using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class SimpleInstancingRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.SimpleInstancing; } }

        public bool TransformsUpdated { get; set; }

        public List<AffineTransform> Transforms { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0SpecularColor { get; set; }

        public SimpleInstancingRenderParameter()
        {
            IsShadowCaster = false;
            IsShadowReceiver = true;
            IsTranslucent = false;
            TransformsUpdated = false;
        }
    }
}
