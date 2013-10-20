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
    public class SimpleInstancingRendererXna : IModelRendererXna
    {
        OpaqueFinalShader Shader = new OpaqueFinalShader();

        Microsoft.Xna.Framework.Matrix[] ModelBones;

        FrustumCulling FrustumCulling = new FrustumCulling();

        public SimpleInstancingRendererXna()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as SimpleInstancingRenderParameter;
            if (param == null)
            {
                return;
            }

            FrustumCulling.UpdateFrustum(param.Camera);

            Shader.InstanceTransforms = param.InstanceTransforms.Where(t =>
            {
                return FrustumCulling.IsCulled(t, 3.0f);
            }).Select(t =>
            {
                return MathUtilXna.ToXnaMatrix(t.WorldMatrix);
            }).ToArray();

            Shader.TransformsUpdated = true;
            Shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            Shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
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
            SetModelBones(model);

            Shader.RenderInstatncedModel();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }

        void SetModelBones(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Array.Resize(ref ModelBones, model.Bones.Count);
            model.CopyAbsoluteBoneTransformsTo(ModelBones);

            Shader.ModelBones = ModelBones;
        }
    }
}
