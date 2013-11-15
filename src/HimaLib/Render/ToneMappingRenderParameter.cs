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
    /// トーンマッピング用のパラメータ
    /// </summary>
    public class ToneMappingRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type
        {
            get { return BillboardRendererType.ToneMapping; }
        }

        public ITexture HDRScene { get; set; }

        public ToneMappingRenderParameter()
        {
            IsHud = true;
        }
    }
}
