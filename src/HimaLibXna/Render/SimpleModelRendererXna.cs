using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class SimpleModelRendererXna : IModelRendererXna
    {
        OpaqueFinalShader Shader = new OpaqueFinalShader();

        public SimpleModelRendererXna()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as SimpleModelRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(param.Transform);
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            
            Shader.Alpha = param.Alpha;
            Shader.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);

            Shader.ShadowEnabled = param.IsShadowReceiver;
            if (Shader.ShadowEnabled)
            {
                Shader.LightViewProjection = MathUtilXna.ToXnaMatrix(param.LightCamera.View * param.LightCamera.Projection);
                Shader.ShadowMap = (param.ShadowMap as ITextureXna).Texture;
            }

            Shader.DiffuseLightMap = (param.DiffuseLightMap as ITextureXna).Texture;
            Shader.SpecularLightMap = (param.SpecularLightMap as ITextureXna).Texture;
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            Shader.RenderModel();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }
    }
}
