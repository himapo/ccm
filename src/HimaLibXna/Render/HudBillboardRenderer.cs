using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class HudBillboardRenderer : IBillboardRendererXna
    {
        public string TextureName { get; set; }

        public float Alpha { get; set; }

        public HimaLib.Math.Vector2 RectOffset { get; set; }

        public HimaLib.Math.Vector2 RectSize { get; set; }

        ConstantShader Shader { get; set; }

        TextureLoader TextureLoader { get; set; }

        public HudBillboardRenderer()
        {
            Shader = new ConstantShader();
            TextureLoader = new TextureLoader();

            RectOffset = HimaLib.Math.Vector2.Zero;
            RectSize = HimaLib.Math.Vector2.Zero;
        }

        public void SetParameter(IBillboardRenderParameter p)
        {
            var param = p as HudBillboardRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.Texture = TextureLoader.Load(param.TextureName);
            Shader.RectOffset = HimaLib.Math.MathUtilXna.ToXnaVector(param.RectOffset);
            Shader.RectSize = HimaLib.Math.MathUtilXna.ToXnaVector(param.RectSize);
            Shader.World = GetWorldMatrix(param.Transform);
            Shader.View = GetViewMatrix();
            Shader.Projection = GetProjMatrix();
            Shader.Alpha = param.Alpha;
        }

        public void Render()
        {
            Shader.RenderBillboard();
        }

        Matrix GetWorldMatrix(HimaLib.Math.AffineTransform transform)
        {
            var result = Matrix.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
            result *= Matrix.CreateRotationZ(transform.Rotation.Z);
            result *= Matrix.CreateTranslation(transform.Translation.X, transform.Translation.Y, transform.Translation.Z);
            return result;
        }

        Matrix GetViewMatrix()
        {
            return Matrix.Identity;
        }

        Matrix GetProjMatrix()
        {
            return new Matrix(
                2.0f / SystemProperty.ScreenWidth, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / SystemProperty.ScreenHeight, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
