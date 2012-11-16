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

        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 RectOffset { get; set; }

        public Vector2 RectSize { get; set; }

        Effect effect;        
        
        VertexPositionTexture[] vertices;

        short[] indices;

        public ConstantShader()
        {
            var contentLoader = new Content.ContentLoader();
            effect = contentLoader.Load<Effect>("Effect/Constant");

            vertices = new VertexPositionTexture[4];
            indices = new short[6] { 0, 1, 2, 2, 1, 3 };
        }

        public void RenderModel()
        {
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
