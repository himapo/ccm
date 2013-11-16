using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class ToneMappingShader
    {
        public Matrix World { get; set; }

        public Matrix View { get; set; }

        public Matrix Projection { get; set; }

        public Texture2D Texture0 { get; set; }

        public Texture2D Texture1 { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        HudBillboard HudBillboard = new HudBillboard();

        public ToneMappingShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/ToneMapping");
        }

        public void SetRenderTargetSize(float x, float y)
        {
            HudBillboard.RectSize = new Vector2(x, y);
            HudBillboard.UpdateVertices();
        }

        public void RenderLuminanceBufferInitial()
        {
            SetUpEffect("SampleAvgLum");

            // 入力テクスチャは320x180
            Effect.Parameters["Texture0"].SetValue(Texture0);

            // 出力テクスチャ64x64に対して
            // 横5倍、縦3倍の320x192の領域をサンプリングする
            Effect.Parameters["SampleOffsets"].SetValue(
                CalcSampleOffsets5x3(
                    HudBillboard.RectSize.X * 5.0f,
                    HudBillboard.RectSize.Y * 3.0f));

            HudBillboard.Render(Effect);
        }

        public void RenderLuminanceBufferIterative()
        {
            SetUpEffect("ResampleAvgLum");
            Effect.Parameters["Texture0"].SetValue(Texture0);

            Effect.Parameters["SampleOffsets"].SetValue(HimaLib.Math.MathUtilXna.CalcSampleOffsets4x4(
                Texture0.Width, Texture0.Height));

            HudBillboard.Render(Effect);
        }

        public void RenderLuminanceBufferFinal()
        {
            SetUpEffect("ResampleAvgLumExp");
            Effect.Parameters["Texture0"].SetValue(Texture0);

            Effect.Parameters["SampleOffsets"].SetValue(HimaLib.Math.MathUtilXna.CalcSampleOffsets4x4(
                Texture0.Width, Texture0.Height));

            HudBillboard.Render(Effect);
        }

        public void RenderFinalPass()
        {
            SetUpEffect("FinalPass");
            Effect.Parameters["Texture0"].SetValue(Texture0);
            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }

        Vector2[] CalcSampleOffsets5x3(float srcWidth, float srcHeight)
        {
            var result = new List<Vector2>();

            float tU = 1.0f / srcWidth;
            float tV = 1.0f / srcHeight;

            int index = 0;
            for (int y = -1; y < 1; y++)
            {
                for (int x = -2; x < 2; x++)
                {
                    result.Add(new Vector2(x * tU, y * tV));
                    index++;
                }
            }

            return result.ToArray();
        }
    }
}
