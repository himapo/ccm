using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class HudBillboard
    {
        public Vector2 RectSize { get; set; }

        VertexPositionTexture[] Vertices;

        short[] Indices;

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        public HudBillboard()
        {
            RectSize = new Vector2(1280.0f, 720.0f);

            Vertices = new VertexPositionTexture[4];
            Indices = new short[6] { 0, 1, 2, 2, 1, 3 };
        }

        public void Render(Effect effect)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    Vertices, 0, 4,
                    Indices, 0, 2);
            }
        }

        public void UpdateVertices()
        {
            var half = 0.5f;
            Vertices[0].Position = new Vector3(-RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            Vertices[1].Position = new Vector3(-RectSize.X * 0.5f - half, RectSize.Y * 0.5f - half, 0.5f);
            Vertices[2].Position = new Vector3(RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            Vertices[3].Position = new Vector3(RectSize.X * 0.5f - half, RectSize.Y * 0.5f - half, 0.5f);

            var uvLeft = 0.0f;
            var uvRight = 1.0f;
            var uvTop = 0.0f;
            var uvBottom = 1.0f;
            Vertices[0].TextureCoordinate = new Vector2(uvLeft, uvBottom);
            Vertices[1].TextureCoordinate = new Vector2(uvLeft, uvTop);
            Vertices[2].TextureCoordinate = new Vector2(uvRight, uvBottom);
            Vertices[3].TextureCoordinate = new Vector2(uvRight, uvTop);
        }
    }
}
