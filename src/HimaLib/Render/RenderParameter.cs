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
    public class RenderParameter
    {
        public CameraBase Camera { get; set; }

        public CameraBase LightCamera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public Matrix Transform { get; set; }

        public bool IsShadowCaster { get; set; }

        public bool IsShadowReceiver { get; set; }

        public bool IsTranslucent { get; set; }

        public bool GBufferEnabled { get; set; }

        public ITexture ShadowMap { get; set; }

        public ITexture DiffuseLightMap { get; set; }

        public ITexture SpecularLightMap { get; set; }
    }
}
