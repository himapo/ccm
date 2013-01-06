using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;

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

        public void SetParameter(SimpleInstancingRenderParameter param)
        {
            SetInstanceTransforms(param.Transforms);

            shader.View = CameraUtil.GetViewMatrix(param.Camera);
            shader.Projection = CameraUtil.GetProjMatrix(param.Camera);
            shader.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);
            shader.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirLight0Direction);
            shader.DirLight0DiffuseColor = MathUtilXna.ToXnaVector(param.DirLight0DiffuseColor);
            shader.DirLight0SpecularColor = MathUtilXna.ToXnaVector(param.DirLight0SpecularColor);
            shader.EyePosition = MathUtilXna.ToXnaVector(param.Camera.Eye);
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

        public void Render(Microsoft.Xna.Framework.Graphics.Model model)
        {
            shader.Model = model;
            SetModelBones(model);

            shader.RenderModel();
        }

        void SetModelBones(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Array.Resize(ref ModelBones, model.Bones.Count);
            model.CopyAbsoluteBoneTransformsTo(ModelBones);

            shader.ModelBones = ModelBones;
        }
    }
}
