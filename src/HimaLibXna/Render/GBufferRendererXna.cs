using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class GBufferRendererXna : IModelRendererXna, IBillboardRendererXna
    {
        GBufferShader Shader = new GBufferShader();

        public GBufferRendererXna()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as GBufferModelRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform.WorldMatrix);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            if (param.Texture != null)
            {
                Shader.Texture = (param.Texture as ITextureXna).Texture;
            }
        }

        public void SetParameter(BillboardRenderParameter p)
        {
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            Shader.RenderModel();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }

        public void Render()
        {
            Shader.RenderBillboard();
        }
    }
}
