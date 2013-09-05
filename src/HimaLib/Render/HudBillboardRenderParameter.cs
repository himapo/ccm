using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class HudBillboardRenderParameter : IBillboardRenderParameter
    {
        public BillboardRendererType Type { get { return BillboardRendererType.Hud; } }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get { return false; } set { } }

        public bool IsTranslucent { get; set; }

        public bool IsHud { get { return true; } set { } }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public string TextureName { get; set; }

        public Vector2 RectOffset { get; set; }

        public Vector2 RectSize { get; set; }
    }
}
