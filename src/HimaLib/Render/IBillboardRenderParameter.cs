using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public enum BillboardRendererType
    {
        Simple,     // 常にカメラの視線と逆を向く
        UI,         // スクリーン座標系で位置を指定
    }

    public interface IBillboardRenderParameter
    {
        BillboardRendererType Type { get; }
    }
}
