using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class LightBufferRenderer : ScreenBillboardRenderer, IModelRendererXna
    {
        LightBufferShader Shader = new LightBufferShader();

        public LightBufferRenderer()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as PointLightRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);

            Shader.PointLightPosition = MathUtilXna.ToXnaVector(param.PointLight.Position);
            Shader.PointLightColor = MathUtilXna.ToXnaColor(param.PointLight.Color).ToVector3();
            Shader.PointLightAttenuationBegin = param.PointLight.AttenuationBegin;
            Shader.PointLightAttenuationEnd = param.PointLight.AttenuationEnd;

            Shader.NormalMap = (param.NormalMap as ITextureXna).Texture;
            Shader.DepthMap = (param.DepthMap as ITextureXna).Texture;

            Shader.IsCameraInLight = 
                (param.Camera.Eye - param.PointLight.Position).Length() 
                < (param.PointLight.AttenuationEnd + param.Camera.Near);

            Shader.LightID = param.LightID;
        }

        public override void SetParameter(BillboardRenderParameter p)
        {
            var param = p as DirectionalLightRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            Shader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            Shader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());

            Shader.DirectionalLightDirection = MathUtilXna.ToXnaVector(param.DirectionalLight.Direction);
            Shader.DirectionalLightColor = MathUtilXna.ToXnaColor(param.DirectionalLight.Color).ToVector3();

            Shader.EyePosition = MathUtilXna.ToXnaVector(param.Camera.Eye);

            Shader.NormalMap = (param.NormalMap as ITextureXna).Texture;
            Shader.DepthMap = (param.DepthMap as ITextureXna).Texture;
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;
            Shader.RenderPoint();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }

        public override void Render()
        {
            Shader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            Shader.RenderDirectional();
        }
    }
}
