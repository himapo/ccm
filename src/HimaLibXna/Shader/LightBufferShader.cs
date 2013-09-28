using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class LightBufferShader
    {
        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public Texture2D NormalDepthMap { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        HudBillboard HudBillboard = new HudBillboard();

        public LightBufferShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/LightBuffer");
        }

        public void SetRenderTargetSize(float x, float y)
        {
            HudBillboard.RectSize = new Vector2(x, y);
            HudBillboard.UpdateVertices();
        }

        public void RenderPoint()
        {
        }

        public void RenderSpot()
        {
        }

        public void RenderDirectional()
        {
            SetUpEffect("Directional");

            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);

            Effect.Parameters["NormalDepthMap"].SetValue(NormalDepthMap);

            Effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            Effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }
    }
}
