using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;
using HimaLib.System;
using HimaLib.Model;

namespace HimaLib.Render
{
    public class WireAABBRenderer : IAABBRendererXna
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        BasicEffect BasicEffect;

        VertexPositionColor[] Vertices;

        short[] Indices;

        public WireAABBRenderer()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            InitEffect();
            InitAABB();
        }

        void InitEffect()
        {
            BasicEffect.VertexColorEnabled = true;
        }

        void InitAABB()
        {
            Vertices = new VertexPositionColor[8];

            SetColor(Microsoft.Xna.Framework.Color.Green);

            Vertices[0].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.0f);
            Vertices[1].Position = new Microsoft.Xna.Framework.Vector3(1.0f, 0.0f, 0.0f);
            Vertices[2].Position = new Microsoft.Xna.Framework.Vector3(1.0f, 1.0f, 0.0f);
            Vertices[3].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 1.0f, 0.0f);
            Vertices[4].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 1.0f);
            Vertices[5].Position = new Microsoft.Xna.Framework.Vector3(1.0f, 0.0f, 1.0f);
            Vertices[6].Position = new Microsoft.Xna.Framework.Vector3(1.0f, 1.0f, 1.0f);
            Vertices[7].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 1.0f, 1.0f);

            Indices = new short[24]
            {
                0, 1,
                1, 2,
                2, 3,
                3, 0,
                4, 5,
                5, 6,
                6, 7,
                7, 4,
                0, 4,
                1, 5,
                2, 6,
                3, 7
            };
        }

        void SetColor(Microsoft.Xna.Framework.Color color)
        {
            for (var i = 0; i < 8; ++i)
            {
                Vertices[i].Color = color;
            }
        }

        public void SetParameter(AABBRenderParameter param)
        {
            BasicEffect.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            BasicEffect.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);

            SetColor(MathUtilXna.ToXnaColor(param.Color));
        }

        public void Render(AABBXna aabb)
        {
            var scaleMat = Matrix.CreateScale(
                    aabb.Width.X,
                    aabb.Width.Y,
                    aabb.Width.Z);
            var transMat = Matrix.CreateTranslation(aabb.Corner);
            BasicEffect.World = MathUtilXna.ToXnaMatrix(scaleMat * transMat);

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, Vertices, 0, Vertices.Length, Indices, 0, 12);
            }
        }
    }
}
