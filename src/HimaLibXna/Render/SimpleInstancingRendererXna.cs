﻿using System;
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
        InstancingPhongShader shader = new InstancingPhongShader();

        Microsoft.Xna.Framework.Matrix[] ModelBones;

        Microsoft.Xna.Framework.Matrix[] InstanceTransforms;

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

            if (param.TransformsUpdated)
            {
                SetInstanceTransforms(param.Transforms);
            }

            shader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            shader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            shader.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);
            shader.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirectionalLights[0].Direction);
            shader.DirLight0DiffuseColor = MathUtilXna.ToXnaVector(param.DirectionalLights[0].Color.ToVector3());
            shader.DirLight0SpecularColor = MathUtilXna.ToXnaVector(param.DirLight0SpecularColor);
            shader.EyePosition = MathUtilXna.ToXnaVector(param.Camera.Eye);
            shader.LightViewProjection = MathUtilXna.ToXnaMatrix(param.LightCamera.View * param.LightCamera.Projection);
            shader.ShadowMap = (param.ShadowMap as ITextureXna).Texture;
        }

        void SetInstanceTransforms(List<AffineTransform> transforms)
        {
            Array.Resize(ref InstanceTransforms, transforms.Count);
            for (var i = 0; i < transforms.Count; ++i)
            {
                InstanceTransforms[i] = MathUtilXna.ToXnaMatrix(transforms[i].WorldMatrix);
            }

            shader.InstanceTransforms = InstanceTransforms;
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            shader.Model = model;
            SetModelBones(model);

            shader.RenderModel();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }

        void SetModelBones(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Array.Resize(ref ModelBones, model.Bones.Count);
            model.CopyAbsoluteBoneTransformsTo(ModelBones);

            shader.ModelBones = ModelBones;
        }
    }
}
