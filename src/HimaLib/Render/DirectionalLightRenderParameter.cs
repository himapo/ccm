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
    /// 平行光源をライトマップに描画するパラメータ
    /// </summary>
    public class DirectionalLightRenderParameter : BillboardRenderParameter
    {
        public override BillboardRendererType Type { get { return BillboardRendererType.DiectionalLight; } }

        public ITexture NormalDepthMap { get; set; }

        public DirectionalLight DirectionalLight { get; set; }

        public DirectionalLightRenderParameter()
        {
            IsHud = true;
        }
    }
}
