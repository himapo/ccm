using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Texture;
using HimaLib.Math;
using HimaLib.Light;

namespace HimaLib.Render
{
    /// <summary>
    /// 縮小バッファ用のパラメータ
    /// </summary>
    public class ScaledBufferRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type
        {
            get { return BillboardRendererType.ScaledBuffer; }
        }

        public ITexture SrcBuffer { get; set; }

        public ScaledBufferRenderParameter()
        {
            IsHud = true;
        }
    }
}
