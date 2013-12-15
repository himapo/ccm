using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Math;
using HimaLib.Texture;
using HimaLib.Debug;

namespace HimaLib.Render
{
    public class GBufferRendererXna : SkinnedModelRendererXna, IBillboardRendererXna
    {
        GBufferShader Shader = new GBufferShader();

        Microsoft.Xna.Framework.Matrix[] ModelBones;

        InstancingType InstancingType;

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

            InstancingType = param.InstancingType;

            if (param.InstancingType == InstancingType.Instanced)
            {
                //LoadProfiler.Instance.BeginMark("GBufferToArray");

                Shader.InstanceTransforms = FrameCacheData.Instance.InstanceTransformsToArray(param.InstanceTransforms);

                //LoadProfiler.Instance.EndMark();
            }
        }

        public void SetParameter(BillboardRenderParameter p)
        {
        }

        public override void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            Shader.Model = model;

            switch (InstancingType)
            {
                case InstancingType.Single:
                    Shader.RenderStaticModel();
                    break;
                case InstancingType.Instanced:
                    //LoadProfiler.Instance.BeginMark("GBufferRender");
                    SetModelBones(model);
                    Shader.RenderInstancedModel();
                    //LoadProfiler.Instance.EndMark();
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
