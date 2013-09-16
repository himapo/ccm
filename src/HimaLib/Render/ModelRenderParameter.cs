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
    }

    public abstract class ModelRenderParameter
    {
        public abstract ModelRendererType Type { get; }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get; set; }

        public bool IsTranslucent { get; set; }

        public virtual ModelRenderParameter ShadowMapRenderParameter
        {
            get { return null; }
        }

        public ModelRenderParameter()
        {
            ShadowEnabled = true;
            IsTranslucent = false;
        }
    }
}
