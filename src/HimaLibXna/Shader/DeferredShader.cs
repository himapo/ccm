using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class DeferredShader
    {
        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public Texture2D AlbedoMap { get; set; }

        public Texture2D PositionMap { get; set; }

        public Texture2D NormalDepthMap { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        HudBillboard HudBillboard = new HudBillboard();

        public DeferredShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/Deferred");
        }

        public void SetRenderTargetSize(float x, float y)
        {
            HudBillboard.RectSize = new Vector2(x, y);
            HudBillboard.UpdateVertices();
        }

        public void RenderBillboard()
        {
            SetUpEffect("Deferred");

            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);

            Effect.Parameters["AlbedoMap"].SetValue(AlbedoMap);
            Effect.Parameters["PositionMap"].SetValue(PositionMap);
            Effect.Parameters["NormalDepthMap"].SetValue(NormalDepthMap);

            Effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            Effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }
    }
}
