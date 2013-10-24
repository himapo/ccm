using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Texture;
using HimaLib.Camera;

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
            Shader.PointLightColor = MathUtilXna.ToXnaVector(param.PointLight.Color.ToVector3());
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
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);   // Viewの逆行列が必要になる
            Shader.Projection = MathUtilXna.ToXnaMatrix(GetScreenProjectionMatrix());

            Shader.DirectionalLightDirection = MathUtilXna.ToXnaVector(param.DirectionalLight.Direction);
            Shader.DirectionalLightColor = MathUtilXna.ToXnaVector(param.DirectionalLight.Color.ToVector3());

            Shader.EyePosition = MathUtilXna.ToXnaVector(param.Camera.Eye);

            if (param.Camera is PerspectiveCamera)
            {
                var perspectiveCamera = param.Camera as PerspectiveCamera;

                var nearPlaneSize = new Vector2();

                nearPlaneSize.Y = param.Camera.Near * MathUtil.Tan(MathUtil.ToRadians(perspectiveCamera.FovY / 2.0f));
                nearPlaneSize.X = nearPlaneSize.Y * perspectiveCamera.Aspect;

                Shader.NearPlaneSize = MathUtilXna.ToXnaVector(nearPlaneSize);
            }
            Shader.Near = param.Camera.Near;
            Shader.Far = param.Camera.Far;

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
