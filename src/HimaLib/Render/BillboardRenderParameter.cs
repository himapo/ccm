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
    public enum BillboardRendererType
    {
        Simple,         // 常にカメラの視線と逆を向く
        Hud,            // スクリーン座標系で位置を指定
        Depth,
        Deferred,       // Deferred Shading のライティングパス
        DiectionalLight,// Deferred Lighting の平行光源マップ生成
        GBuffer,
    }

    public abstract class BillboardRenderParameter : RenderParameter
    {
        public abstract BillboardRendererType Type { get; }
        
        public bool IsHud { get; set; }

        public BillboardRenderParameter()
        {
            IsShadowCaster = true;
            IsShadowReceiver = true;
            IsTranslucent = false;

            IsHud = false;
        }
    }
}
