using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    /// <summary>
    /// 不透明物の最終パスシェーダ
    /// ライトバッファを参照してジオメトリを再度描画する
    /// </summary>
    public class OpaqueFinalShader
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public Texture2D ModelTexture { get; set; }

        public Matrix World { get; set; }

        public Matrix[] ModelBones { get; set; }

        public Matrix[] InstanceTransforms { get; set; }

        public bool TransformsUpdated { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public bool ShadowEnabled { get; set; }

        public Texture2D ShadowMap { get; set; }

        public Matrix LightViewProjection { get; set; }

        public Texture2D DiffuseLightMap { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        InstancedVertexBuffer InstancedVertexBuffer = new InstancedVertexBuffer();

        public OpaqueFinalShader()
        {
            ModelTexture = new Texture2D(GraphicsDevice, 32, 32);

            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;
            Alpha = 1.0f;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/OpaqueFinal");
        }

        public void RenderModel()
        {
            if (ShadowEnabled)
            {
                SetUpEffect("StaticShadow");
            }
            else
            {
                SetUpEffect("Static");
            }

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

                    CopyMaterial(part.Effect as BasicEffect);

                    foreach (var pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                            0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public void RenderInstatncedModel()
        {
            if (InstanceTransforms.Length == 0)
                return;

            InstancedVertexBuffer.Setup(InstanceTransforms);

            if (ShadowEnabled)
            {
                SetUpEffect("InstancingShadow");
            }
            else
            {
                SetUpEffect("Instancing");
            }

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(part.VertexBuffer, part.VertexOffset, 0),
                        new VertexBufferBinding(InstancedVertexBuffer.VertexBuffer, 0, 1)
                        );

                    GraphicsDevice.Indices = part.IndexBuffer;

                    CopyMaterial(part.Effect as BasicEffect);

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

        public void RenderBillboard()
        {
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.Parameters["LightViewProjection"].SetValue(LightViewProjection);

            Effect.Parameters["ModelTexture"].SetValue(ModelTexture);
            Effect.Parameters["ShadowMap"].SetValue(ShadowMap);
            Effect.Parameters["DiffuseLightMap"].SetValue(DiffuseLightMap);

            Effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }

        void CopyMaterial(BasicEffect src)
        {
            if (src == null)
                return;

            Effect.Parameters["DiffuseColor"].SetValue(src.DiffuseColor);
            Effect.Parameters["Alpha"].SetValue(src.Alpha * Alpha);
        }
    }
}
