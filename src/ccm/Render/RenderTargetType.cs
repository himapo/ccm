using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Render
{
    public enum RenderTargetType
    {
        BackBuffer = 0,
        ShadowMap0,
        DiffuseLightMap,
        SpecularLightMap,
        GBuffer0,
        GBuffer1,
        GBuffer2,
        GBuffer3,

        // トーンマッピング関連
        HDRBuffer,          // トーンマッピング前のHDRバッファ
        ScaledBuffer,       // 上の縮小バッファ
        LuminanceBuffer64,  // 64x64の平均輝度バッファ
        LuminanceBuffer16,
        LuminanceBuffer4,
        LuminanceBuffer1,
        AdaptedLuminanceBuffer0,    // 明順応表現用のダブルバッファ
        AdaptedLuminanceBuffer1,

        Length,
    }
}
