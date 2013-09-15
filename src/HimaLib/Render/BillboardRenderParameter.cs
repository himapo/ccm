using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public enum BillboardRendererType
    {
        Simple,     // 常にカメラの視線と逆を向く
        Hud,         // スクリーン座標系で位置を指定
        Depth,
    }

    public abstract class BillboardRenderParameter
    {
        public abstract BillboardRendererType Type { get; }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public bool ShadowEnabled { get; set; }

        public bool IsTranslucent { get; set; }

        public bool IsHud { get; set; }

        public BillboardRenderParameter()
        {
            ShadowEnabled = true;
            IsTranslucent = false;
            IsHud = false;
        }
    }
}
