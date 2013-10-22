using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class GBufferShader
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public Matrix World { get; set; }

        public Matrix[] ModelBones { get; set; }

        public Matrix[] InstanceTransforms { get; set; }

        public bool TransformsUpdated { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public Texture2D BoneRotationTexture { get; set; }

        public Texture2D BoneTranslationTexture { get; set; }

        public Vector2 BoneTextureSize { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        InstancedVertexBuffer InstancedVertexBuffer = new InstancedVertexBuffer();

        public GBufferShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/GBufferND");
        }

        public void RenderStaticModel()
        {
            SetUpEffect("Static");
            RenderModelCommon();
        }

        public void RenderInstancedModel()
        {
            if (InstanceTransforms.Length == 0)
                return;

            InstancedVertexBuffer.Setup(InstanceTransforms);

            SetUpEffect("Instancing");

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(part.VertexBuffer, part.VertexOffset, 0),
                        new VertexBufferBinding(InstancedVertexBuffer.VertexBuffer, 0, 1)
                        );

                    GraphicsDevice.Indices = part.IndexBuffer;

                    Effect.Parameters["World"].SetValue(ModelBones[mesh.ParentBone.Index]);

                    foreach (var pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawInstancedPrimitives(
                            PrimitiveType.TriangleList,
                            0,
                            0,
                            part.NumVertices,
                            part.StartIndex,
                            part.PrimitiveCount,
                            InstanceTransforms.Length);
                    }
                }
            }
        }

        public void RenderDynamicModel()
        {
            Effect.Parameters["BoneRotationTexture"].SetValue(BoneRotationTexture);
            Effect.Parameters["BoneTranslationTexture"].SetValue(BoneTranslationTexture);
            Effect.Parameters["BoneTextureSize"].SetValue(BoneTextureSize);

            SetUpEffect("Skinning");
            RenderModelCommon();
        }

        void RenderModelCommon()
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

                    CopyMaterial(part.Effect);

                    foreach (var pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                            0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public void RenderBillboard()
        {
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }

        void CopyMaterial(Effect src)
        {
            var specularPower = src.Parameters.FirstOrDefault((param) =>
            {
                return param.Name == "SpecularPower";
            });

            if (specularPower == null)
            {
                return;
            }
            
            Effect.Parameters["Shininess"].SetValue(specularPower.GetValueSingle());
        }
    }
}
