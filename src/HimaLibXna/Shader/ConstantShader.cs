using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Shader
{
    public class ConstantShader : Graphics.GraphicsDeviceUser
    {
        public Model Model { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 RectOffset { get; set; }

        private Vector2 rectSize;
        public Vector2 RectSize
        {
            get
            {
                if (rectSize.X == 0.0f || rectSize.Y == 0.0f)
                {
                    return new Vector2(Texture.Width, Texture.Height);
                }
                else
                {
                    return rectSize;
                }
            }
            set
            {
                rectSize = value;
            }
        }

        VertexPositionTexture[] vertices;

        short[] indices;


        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }

        Effect effect;

        public ConstantShader()
        {
            Texture = new Texture2D(GraphicsDevice, 32, 32);
            RectOffset = new Vector2(0.0f, 0.0f);
            rectSize = new Vector2(0.0f, 0.0f);

            vertices = new VertexPositionTexture[4];
            indices = new short[6] { 0, 1, 2, 2, 1, 3 };

            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;
            Alpha = 1.0f;

            var contentLoader = new Content.EffectLoader();
            effect = contentLoader.Load("Effect/Constant");
        }

        public void RenderModel()
        {
            SetUpEffect();

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
            SetUpVertices();

            SetUpEffect();

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    vertices, 0, 4,
                    indices, 0, 2);
            }
        }

        void SetUpVertices()
        {
            var half = 0.5f;
            vertices[0].Position = new Vector3(-RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            vertices[1].Position = new Vector3(-RectSize.X * 0.5f - half, RectSize.Y * 0.5f - half, 0.5f);
            vertices[2].Position = new Vector3(RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            vertices[3].Position = new Vector3(RectSize.X * 0.5f - half, RectSize.Y * 0.5f - half, 0.5f);

            var uvLeft = RectOffset.X / Texture.Width;
            var uvRight = (RectOffset.X + RectSize.X) / Texture.Width;
            var uvTop = RectOffset.Y / Texture.Height;
            var uvBottom = (RectOffset.Y + RectSize.Y) / Texture.Height;
            vertices[0].TextureCoordinate = new Vector2(uvLeft, uvBottom);
            vertices[1].TextureCoordinate = new Vector2(uvLeft, uvTop);
            vertices[2].TextureCoordinate = new Vector2(uvRight, uvBottom);
            vertices[3].TextureCoordinate = new Vector2(uvRight, uvTop);
        }

        void SetUpEffect()
        {
            effect.Parameters["Alpha"].SetValue(Alpha);
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["DiffuseMap"].SetValue(Texture);

            effect.CurrentTechnique = effect.Techniques["TechniqueTx"];
        }
    }
}
