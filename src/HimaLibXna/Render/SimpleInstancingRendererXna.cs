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
        InstancingPhongShader shader = new InstancingPhongShader();

        Microsoft.Xna.Framework.Matrix[] ModelBones;

        /// <summary>
        /// 視錐台を覆うAABBのXYZそれぞれの最大座標
        /// </summary>
        Vector3 MinFrustumAABB;

        /// <summary>
        /// 視錐台を覆うAABBのXYZそれぞれの最小座標
        /// </summary>
        Vector3 MaxFrustumAABB;

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

            UpdateFrustum(param.Camera);

            shader.InstanceTransforms = param.Transforms.Where(t =>
            {
                return FrustumCulling(t, 3.0f);
            }).Select(t =>
            {
                return MathUtilXna.ToXnaMatrix(t.WorldMatrix);
            }).ToArray();

            shader.TransformsUpdated = true;
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

        void UpdateFrustum(CameraBase camera)
        {
            var invViewProj = Matrix.Invert(camera.View * camera.Projection);

            var projCorners = new Vector3[8]
            {
                new Vector3(-1, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(-1, -1, 0),
                new Vector3(1, -1, 0),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(-1, -1, 1),
                new Vector3(1, -1, 1),
            };

            var worldCorners = projCorners.Select(c =>
            {
                return Vector3.TransformCoord(c, invViewProj);
            });

            // 計算を軽くするため、視錐台に外接するAABBでカリングする
            MinFrustumAABB = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            MaxFrustumAABB = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var frustumCorner in worldCorners)
            {
                MinFrustumAABB.X = MathUtil.Min(MinFrustumAABB.X, frustumCorner.X);
                MinFrustumAABB.Y = MathUtil.Min(MinFrustumAABB.Y, frustumCorner.Y);
                MinFrustumAABB.Z = MathUtil.Min(MinFrustumAABB.Z, frustumCorner.Z);

                MaxFrustumAABB.X = MathUtil.Max(MaxFrustumAABB.X, frustumCorner.X);
                MaxFrustumAABB.Y = MathUtil.Max(MaxFrustumAABB.Y, frustumCorner.Y);
                MaxFrustumAABB.Z = MathUtil.Max(MaxFrustumAABB.Z, frustumCorner.Z);
            }
        }

        bool FrustumCulling(AffineTransform transform, float margin)
        {
            if (transform.Translation.X > MaxFrustumAABB.X + margin)
                return false;

            if (transform.Translation.Y > MaxFrustumAABB.Y + margin)
                return false;

            if (transform.Translation.Z > MaxFrustumAABB.Z + margin)
                return false;

            if (transform.Translation.X < MinFrustumAABB.X - margin)
                return false;

            if (transform.Translation.Y < MinFrustumAABB.Y - margin)
                return false;

            if (transform.Translation.Z < MinFrustumAABB.Z - margin)
                return false;

            return true;
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
