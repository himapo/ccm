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
            SetUpEffect("DownScale4x4");
            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);

            Effect.Parameters["SrcBuffer"].SetValue(SrcBuffer);
            Effect.Parameters["SampleOffsets"].SetValue(CalcSampleOffsets4x4());

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }

        Vector2[] CalcSampleOffsets4x4()
        {
            var result = new List<Vector2>();

            float tU = 1.0f / SrcBuffer.Width;
            float tV = 1.0f / SrcBuffer.Height;

            int index = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    result.Add(new Vector2((x - 1.5f) * tU, (y - 1.5f) * tV));
                    index++;
                }
            }

            return result.ToArray();
        }
    }
}
