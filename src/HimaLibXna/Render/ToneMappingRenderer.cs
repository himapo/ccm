using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class ToneMappingRenderer : ScreenBillboardRenderer
    {
        ToneMappingRenderParameter RenderParam;

        ScaledBufferShader ScaledBufferShader = new ScaledBufferShader();

        ToneMappingShader ToneMappingShader = new ToneMappingShader();

        IRenderDevice RenderDevice { get { return RenderParam.RenderDevice; } }

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

            RenderParam = param;
        }

        public override void Render()
        {
            RenderScaledBuffer();

            RenderLuminanceBuffer();

            RenderAdaptedLuminanceBuffer();

            RenderFinalPass();
        }

        void RenderScaledBuffer()
        {
            RenderDevice.SetRenderTarget(RenderParam.ScaledBufferIndex);

            RenderDevice.ClearAll(Color.Purple);

            ScaledBufferShader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ScaledBufferShader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ScaledBufferShader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());

            ScaledBufferShader.SrcBuffer = (RenderParam.HDRScene as ITextureXna).Texture;

            //ScaledBufferShader.SetRenderTargetSize(RenderParam.ScaledBufferSize.X, RenderParam.ScaledBufferSize.Y);
            ScaledBufferShader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            ScaledBufferShader.Render();
        }

        void RenderLuminanceBuffer()
        {

        }

        void RenderAdaptedLuminanceBuffer()
        {

        }

        void RenderFinalPass()
        {
            RenderDevice.SetRenderTarget(RenderParam.RenderTargetIndex);

            RenderDevice.ClearAll(Color.Purple);

            ToneMappingShader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());

            ToneMappingShader.HDRScene = (RenderParam.HDRScene as ITextureXna).Texture;

            ToneMappingShader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            ToneMappingShader.RenderFinalPass();
        }
    }
}
