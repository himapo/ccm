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
    public class SimpleBillboardRenderParameter : IBillboardRenderParameter
    {
        public BillboardRendererType Type { get { return BillboardRendererType.Simple; } }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get; set; }

        public bool IsTranslucent { get; set; }

        public bool IsHud { get { return false; } set { } }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public ITexture Texture { get; set; }
    }
}
