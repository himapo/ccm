using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;
using HimaLib.Content;
using HimaLib.System;
using HimaLib.Texture;

namespace HimaLib.Render
{
    /// <summary>
    /// モデルに設定されたエフェクトでそのまま描画するレンダラ
    /// </summary>
    public class DefaultModelRendererXna : SkinnedModelRendererXna
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        DefaultModelRenderParameter RenderParam = new DefaultModelRenderParameter();

        TextureLoader TextureLoader = new TextureLoader();

        public DefaultModelRendererXna()
        {
        }

        public override void SetParameter(ModelRenderParameter p)
        {
            var param = p as DefaultModelRenderParameter;
            if (param == null)
            {
                return;
            }

            RenderParam = param;

            RenderParam.ParametersMatrix["View"] = RenderParam.Camera.View;
            RenderParam.ParametersMatrix["Projection"] = RenderParam.Camera.Projection;

            if (RenderParam.IsShadowReceiver)
            {
                RenderParam.ParametersMatrix["LightViewProjection"] = RenderParam.LightCamera.View * RenderParam.LightCamera.Projection;
                RenderParam.ParametersTexture["ShadowMap"] = (RenderParam.ShadowMap as ITextureXna).Texture;
            }

            RenderParam.ParametersTexture["DiffuseLightMap"] = (RenderParam.DiffuseLightMap as ITextureXna).Texture;
            RenderParam.ParametersTexture["SpecularLightMap"] = (RenderParam.SpecularLightMap as ITextureXna).Texture;
        }

        public override void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            RenderCommon(model);
        }

        public override void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            RenderParam.ParametersTexture["BoneRotationTexture"] = BoneRotationTexture;
            RenderParam.ParametersTexture["BoneTranslationTexture"] = BoneTranslationTexture;
            RenderParam.ParametersVector2["BoneTextureSize"] = new Vector2(BoneTextureSize.X, BoneTextureSize.Y);

            RenderCommon(model);
        }

        void RenderCommon(Microsoft.Xna.Framework.Graphics.Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    foreach (var dst in part.Effect.Parameters)
                    {
                        if (dst.Name == "TechniqueName")
                        {
                            var techniqueName = dst.GetValueString();
                            part.Effect.CurrentTechnique = part.Effect.Techniques[techniqueName];
                        }
                        else
                        {
                            SetParameter(dst);
                        }
                    }

                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

                    foreach (var pass in part.Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                            0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        void SetParameter(EffectParameter dst)
        {
            switch(dst.ParameterClass)
            {
                case EffectParameterClass.Matrix:
                    SetParameterMatrix(dst);
                    break;
                case EffectParameterClass.Object:
                    switch (dst.ParameterType)
                    {
                        case EffectParameterType.Texture:
                            SetParameterTexture(dst);
                            break;
                        default:
                            break;
                    }
                    break;
                case EffectParameterClass.Scalar:
                    switch (dst.ParameterType)
                    {
                        case EffectParameterType.Bool:
                            SetParameterBoolean(dst);
                            break;
                        case EffectParameterType.Int32:
                            SetParameterInt32(dst);
                            break;
                        case EffectParameterType.Single:
                            SetParameterSingle(dst);
                            break;
                        default:
                            break;
                    }
                    break;
                case EffectParameterClass.Struct:
                    break;
                case EffectParameterClass.Vector:
                    switch (dst.ColumnCount)
                    {
                        case 2:
                            SetParameterVector2(dst);
                            break;
                        case 3:
                            SetParameterVector3(dst);
                            break;
                        case 4:
                            SetParameterVector4(dst);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        void SetParameterMatrix(EffectParameter dst)
        {
            Matrix value;
            if (RenderParam.ParametersMatrix.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(MathUtilXna.ToXnaMatrix(value));
            }
        }

        void SetParameterTexture(EffectParameter dst)
        {
            string value;
            if (RenderParam.ParametersTextureName.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(TextureLoader.Load(value));
                return;
            }

            object texture;
            if (RenderParam.ParametersTexture.TryGetValue(dst.Name, out texture))
            {
                if (texture is Microsoft.Xna.Framework.Graphics.Texture)
                {
                    dst.SetValue(texture as Microsoft.Xna.Framework.Graphics.Texture);
                }
                else if (texture is ITextureXna)
                {
                    dst.SetValue((texture as ITextureXna).Texture);
                }
            }
        }

        void SetParameterBoolean(EffectParameter dst)
        {
            bool value;
            if (RenderParam.ParametersBoolean.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(value);
            }
        }

        void SetParameterInt32(EffectParameter dst)
        {
            int value;
            if (RenderParam.ParametersInt32.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(value);
            }
        }

        void SetParameterSingle(EffectParameter dst)
        {
            float value;
            if (RenderParam.ParametersSingle.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(value);
            }
        }

        void SetParameterVector2(EffectParameter dst)
        {
            Vector2 value;
            if (RenderParam.ParametersVector2.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(MathUtilXna.ToXnaVector(value));
            }
        }

        void SetParameterVector3(EffectParameter dst)
        {
            Vector3 value;
            if (RenderParam.ParametersVector3.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(MathUtilXna.ToXnaVector(value));
            }
        }

        void SetParameterVector4(EffectParameter dst)
        {
            Vector4 value;
            if (RenderParam.ParametersVector4.TryGetValue(dst.Name, out value))
            {
                dst.SetValue(MathUtilXna.ToXnaVector(value));
            }
        }
    }
}
