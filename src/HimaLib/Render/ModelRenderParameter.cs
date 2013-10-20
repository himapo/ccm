using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Texture;
using HimaLib.Math;

namespace HimaLib.Render
{
    public enum ModelRendererType
    {
        Default,
        Simple,
        SimpleInstancing,
        Toon,
        Depth,
        GBuffer,
        PointLight,
    }

    public enum ModelType
    {
        Static,
        InstancedStatic,
        Dynamic,
        InstancedDynamic,
    }

    public abstract class ModelRenderParameter : RenderParameter
    {
        public abstract ModelRendererType Type { get; }

        public ModelType ModelType { get; set; }

        public bool TransformsUpdated { get; set; }

        public List<AffineTransform> InstanceTransforms { get; set; }

        public ModelRenderParameter()
        {
            IsShadowCaster = true;
            IsShadowReceiver = true;
            IsTranslucent = false;
            GBufferEnabled = true;

            ModelType = ModelType.Static;
        }
    }
}
