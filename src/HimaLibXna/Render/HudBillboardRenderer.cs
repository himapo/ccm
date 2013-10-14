using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Content;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class HudBillboardRenderer : IBillboardRendererXna
    {
        ConstantShader Shader { get; set; }

        public HudBillboardRenderer()
        {
            Shader = new ConstantShader();
        }

        public void SetParameter(BillboardRenderParameter p)
        {
            var param = p as HudBillboardRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.Texture = (param.Texture as ITextureXna).Texture;
            Shader.RectOffset = MathUtilXna.ToXnaVector(param.RectOffset);
            Shader.RectSize = MathUtilXna.ToXnaVector(param.RectSize);
            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform);
            Shader.View = MathUtilXna.ToXnaMatrix(GetViewMatrix());
            Shader.Projection = MathUtilXna.ToXnaMatrix(GetProjMatrix());
            Shader.Alpha = param.Alpha;
        }

        public void Render()
        {
            Shader.RenderBillboard();
        }

        Matrix GetWorldMatrix(AffineTransform transform)
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
