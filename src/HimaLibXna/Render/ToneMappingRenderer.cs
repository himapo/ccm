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

            ScaledBufferShader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            ScaledBufferShader.Render();
        }

        void RenderLuminanceBuffer()
        {
            ToneMappingShader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());
            ToneMappingShader.SetRenderTargetSize(ScreenWidth, ScreenHeight);

            foreach (var item in RenderParam.LuminanceBufferIndices.Select((val, index) => new { val, index }))
            {
                RenderDevice.SetRenderTarget(item.val);

                RenderDevice.ClearAll(Color.Purple);

                if (item.index == 0)
                {
                    // 1パス目は縮小バッファを入力
                    ToneMappingShader.Texture0 = (RenderParam.ScaledBuffer as ITextureXna).Texture;
                    ToneMappingShader.RenderLuminanceBufferInitial();
                }
                else
                {
                    // 2パス目移行は前の出力を入力
                    ToneMappingShader.Texture0 = (RenderParam.LuminanceBuffers.ElementAt(item.index - 1) as ITextureXna).Texture;

                    if (item.index == RenderParam.LuminanceBufferIndices.Count() - 1)
                    {
                        ToneMappingShader.RenderLuminanceBufferFinal();
                    }
                    else
                    {
                        ToneMappingShader.RenderLuminanceBufferIterative();
                    }
                }
            }
        }

        void RenderAdaptedLuminanceBuffer()
        {

        }

        void RenderFinalPass()
        {
            ToneMappingShader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            ToneMappingShader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());
            ToneMappingShader.SetRenderTargetSize(ScreenWidth, ScreenHeight);

            RenderDevice.SetRenderTarget(RenderParam.RenderTargetIndex);

            RenderDevice.ClearAll(Color.Purple);

            ToneMappingShader.Texture0 = (RenderParam.HDRScene as ITextureXna).Texture;
            ToneMappingShader.Texture1 = (RenderParam.LuminanceBuffers.Last() as ITextureXna).Texture;

            ToneMappingShader.RenderFinalPass();
        }
    }
}
