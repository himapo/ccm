using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class DepthShader
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public Matrix World { get; set; }

        public Matrix View { get; set; }

        public Matrix Projection { get; set; }

        public Texture2D BoneRotationTexture { get; set; }

        public Texture2D BoneTranslationTexture { get; set; }

        public Vector2 BoneTextureSize { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect effect;

        public DepthShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            effect = contentLoader.Load("Effect/Depth");
        }

        public void RenderStaticModel()
        {
            SetUpEffect("RenderDepthStatic");

            RenderModelCommon();
        }

        public void RenderDynamicModel()
        {
            effect.Parameters["BoneRotationTexture"].SetValue(BoneRotationTexture);
            effect.Parameters["BoneTranslationTexture"].SetValue(BoneTranslationTexture);
            effect.Parameters["BoneTextureSize"].SetValue(BoneTextureSize);

            SetUpEffect("RenderDepthSkinning");

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

                    foreach (var pass in effect.CurrentTechnique.Passes)
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
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);

            effect.CurrentTechnique = effect.Techniques[techniqueName];
        }
    }
}
