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

        Microsoft.Xna.Framework.Matrix[] ModelBones;

        FrustumCulling FrustumCulling = new FrustumCulling();

        ModelType ModelType;

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

            ModelType = param.ModelType;

            if (param.ModelType == ModelType.InstancedStatic)
            {
                FrustumCulling.UpdateFrustum(param.Camera);

                Shader.InstanceTransforms = param.InstanceTransforms.Where(t =>
                {
                    return FrustumCulling.IsCulled(t, 3.0f);
                }).Select(t =>
                {
                    return MathUtilXna.ToXnaMatrix(t.WorldMatrix);
                }).ToArray();
            }
        }

        public void SetParameter(BillboardRenderParameter p)
        {
        }

        public override void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            switch (ModelType)
            {
                case ModelType.Static:
                    Shader.RenderStaticModel();
                    break;
                case ModelType.InstancedStatic:
                    SetModelBones(model);
                    Shader.RenderInstancedModel();
                    break;
            }
        }

        void SetModelBones(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Array.Resize(ref ModelBones, model.Bones.Count);
            model.CopyAbsoluteBoneTransformsTo(ModelBones);

            Shader.ModelBones = ModelBones;
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
