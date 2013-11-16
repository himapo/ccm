using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class ScaledBufferShader
    {
        public Matrix World { get; set; }

        public Matrix View { get; set; }

        public Matrix Projection { get; set; }

        public Texture2D SrcBuffer { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        HudBillboard HudBillboard = new HudBillboard();

        Vector2[] SampleOffsets;

        public ScaledBufferShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/ScaledBuffer");
        }

        public void SetRenderTargetSize(float x, float y)
        {
            HudBillboard.RectSize = new Vector2(x, y);
            HudBillboard.UpdateVertices();
        }

        public void Render()
        {
            SetUpEffect("DownScale4x4PseudoHDR");
            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);

            Effect.Parameters["SrcBuffer"].SetValue(SrcBuffer);

            if (SampleOffsets == null)
            {
                CalcSampleOffsets();
            }

            Effect.Parameters["SampleOffsets"].SetValue(SampleOffsets);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }

        void CalcSampleOffsets()
        {
            var sampleOffsets = HimaLib.Math.MathUtil.CalcSampleOffsets4x4(SrcBuffer.Width, SrcBuffer.Height);
            SampleOffsets = sampleOffsets.Select((v) =>
            {
                return HimaLib.Math.MathUtilXna.ToXnaVector(v);
            }).ToArray();
        }
    }
}
