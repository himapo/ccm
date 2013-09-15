using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class DepthRendererXna : IModelRendererXna, IBillboardRendererXna
    {
        DepthShader Shader = new DepthShader();

        public DepthRendererXna()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as DepthModelRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform.WorldMatrix);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
        }

        public void SetParameter(BillboardRenderParameter p)
        {
            var param = p as DepthBillboardRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform.WorldMatrix);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
        }

        public void Render(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            Shader.RenderModel();
        }

        public void Render()
        {
            Shader.RenderBillboard();
        }
    }
}
