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

        public IRenderDevice RenderDevice { get; set; }

        public ITexture HDRScene { get; set; }

        public int ScaledBufferIndex { get; set; }

        public Vector2 ScaledBufferSize { get; set; }

        public IEnumerable<int> LuminanceBufferIndices { get; set; }

        public int[] AdaptedLuminanceBufferIndices { get; set; }

        public int RenderTargetIndex { get; set; }

        public ToneMappingRenderParameter()
        {
            IsHud = true;
        }
    }
}
