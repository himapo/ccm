using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class DepthRendererXna : SkinnedModelRendererXna, IBillboardRendererXna
    {
        DepthShader Shader = new DepthShader();

        public DepthRendererXna()
        {
        }

        public override void SetParameter(ModelRenderParameter p)
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

        public override void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            Shader.RenderStaticModel();
        }

        public override void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            Shader.BoneRotationTexture = BoneRotationTexture;
            Shader.BoneTranslationTexture = BoneTranslationTexture;
            Shader.BoneTextureSize = BoneTextureSize;

            Shader.RenderDynamicModel();
        }

        public void Render()
        {
            Shader.RenderBillboard();
        }
    }
}
