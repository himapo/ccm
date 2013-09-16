using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class SimpleModelRenderParameter : ModelRenderParameter
    {
        public override ModelRendererType Type { get { return ModelRendererType.Simple; } }

        AffineTransform transform;
        public AffineTransform Transform
        {
            get { return transform; }
            set
            {
                transform = value;
                DepthModelRenderParameter.Transform = value;
            }
        }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public override ModelRenderParameter ShadowMapRenderParameter
        {
            get
            {
                return DepthModelRenderParameter;
            }
        }

        DepthModelRenderParameter DepthModelRenderParameter = new DepthModelRenderParameter();

        public SimpleModelRenderParameter()
        {
            ShadowEnabled = true;
            IsTranslucent = false;
            Transform = new AffineTransform();
            Alpha = 1.0f;
            AmbientLightColor = Vector3.One * 0.4f;
        }
    }
}
