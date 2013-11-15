using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Texture;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class ToneMappingRenderer : ScreenBillboardRenderer
    {
        ToneMappingShader Shader = new ToneMappingShader();

        public ToneMappingRenderer()
        {
        }

        public override void SetParameter(BillboardRenderParameter p)
        {
            var param = p as ToneMappingRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);   // Viewの逆行列が必要になる
            Shader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());

            Shader.HDRScene = (param.HDRScene as ITextureXna).Texture;
        }

        public override void Render()
        {
            Shader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            Shader.RenderFinalPass();
        }
    }
}
