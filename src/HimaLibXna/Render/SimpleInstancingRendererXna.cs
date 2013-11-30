using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Texture;
using HimaLib.Debug;

namespace HimaLib.Render
{
    public class SimpleInstancingRendererXna : IModelRendererXna
    {
        OpaqueFinalShader Shader = new OpaqueFinalShader();

        Microsoft.Xna.Framework.Matrix[] ModelBones;

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

            LoadProfiler.Instance.BeginMark("OpaqueToArray");

            Shader.InstanceTransforms = param.InstanceTransforms.Select(matrix =>
            {
                return MathUtilXna.ToXnaMatrix(matrix);
            }).ToArray();

            LoadProfiler.Instance.EndMark();

            Shader.TransformsUpdated = param.TransformsUpdated;
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
            LoadProfiler.Instance.BeginMark("OpaqueRender");

            Shader.Model = model;
            SetModelBones(model);

            Shader.RenderInstatncedModel();

            LoadProfiler.Instance.EndMark();
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
