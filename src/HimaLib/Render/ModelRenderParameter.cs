using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;

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
    }

    public abstract class ModelRenderParameter
    {
        public abstract ModelRendererType Type { get; }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool IsShadowCaster { get; set; }

        public bool IsShadowReceiver { get; set; }

        public bool IsTranslucent { get; set; }

        public ModelRenderParameter()
        {
            IsShadowCaster = true;
            IsShadowReceiver = true;
            IsTranslucent = false;
        }
    }
}
