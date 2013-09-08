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

    public interface IBillboardRenderParameter
    {
        BillboardRendererType Type { get; }

        CameraBase Camera { get; set; }

        List<DirectionalLight> DirectionalLights { get; set; }

        bool ShadowEnabled { get; set; }

        bool IsTranslucent { get; set; }

        bool IsHud { get; set; }
    }
}
