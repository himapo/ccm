using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class GBufferRendererXna : SkinnedModelRendererXna, IBillboardRendererXna
    {
        GBufferShader Shader = new GBufferShader();

        public GBufferRendererXna()
        {
        }

        public override void SetParameter(ModelRenderParameter p)
        {
            var param = p as GBufferModelRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform);
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
