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

        public ITexture ScaledBuffer { get; set; }

        public IEnumerable<int> LuminanceBufferIndices { get; set; }

        public IEnumerable<ITexture> LuminanceBuffers { get; set; }

        /// <summary>
        /// 明順応用の補間輝度を格納するダブルバッファ
        /// </summary>
        public int[] AdaptedLuminanceBufferIndices { get; set; }

        public ITexture[] AdaptedLuminanceBuffers { get; set; }

        public int RenderTargetIndex { get; set; }

        public float Exposure { get; set; }

        public ToneMappingRenderParameter()
        {
            IsHud = true;
        }
    }
}
