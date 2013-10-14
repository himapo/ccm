using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;
using HimaLib.System;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class WireSphereRenderer : ISphereRendererXna
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        BasicEffect BasicEffect;

        VertexPositionColor[] Vertices;

        short[] Indices;

        public WireSphereRenderer()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            InitEffect();
            InitSphere();
        }

        void InitEffect()
        {
            BasicEffect.VertexColorEnabled = true;
        }

        void InitSphere()
        {
            // 球の頂点初期化
            Vertices = new VertexPositionColor[12 * 5 + 2];

            for (var i = 0; i < 5; ++i)
            {
                var theta = MathUtil.ToRadians(30.0f * (i + 1));
                for (var j = 0; j < 12; ++j)
                {
                    var phi = MathUtil.ToRadians(30.0f * j);
                    Vertices[i * 12 + j].Position = new Microsoft.Xna.Framework.Vector3(
                        MathUtil.Sin(theta) * MathUtil.Sin(phi),
                        MathUtil.Cos(theta),
                        MathUtil.Sin(theta) * MathUtil.Cos(phi));
                    Vertices[i * 12 + j].Color = Microsoft.Xna.Framework.Color.Red;
                }
            }
            Vertices[60].Position = Microsoft.Xna.Framework.Vector3.UnitY;
            Vertices[60].Color = Microsoft.Xna.Framework.Color.Red;
            Vertices[61].Position = -Microsoft.Xna.Framework.Vector3.UnitY;
            Vertices[61].Color = Microsoft.Xna.Framework.Color.Red;

            Indices = new short[13 * (6 + 5)];

            // 垂直に6枚の円
            for (var i = 0; i < 6; ++i)
            {
                Indices[i * 13 + 0] = 60;
                Indices[i * 13 + 6] = 61;
                Indices[i * 13 + 12] = 60;
                for (var j = 0; j < 5; ++j)
                {
                    Indices[i * 13 + j + 1] = (short)(j * 12 + i);
                    Indices[i * 13 + 11 - j] = (short)(j * 12 + i + 6);
                }
            }

            // 水平に5枚の円
            var offset = 13 * 6;
            for (var i = 0; i < 5; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    Indices[i * 13 + j + offset] = (short)(i * 12 + j);
                }
                Indices[i * 13 + 12 + offset] = Indices[i * 13 + offset];
            }
        }

        public void SetParameter(SphereRenderParameter p)
        {
            var param = p as WireSphereRenderParameter;
            if (param == null)
            {
                return;
            }

            BasicEffect.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            BasicEffect.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
        }

        public void Render(Sphere sphere)
        {
            var scaleMat = Matrix.CreateScale(sphere.Radius);
            var transMat = Matrix.CreateTranslation(sphere.Position);
            BasicEffect.World = MathUtilXna.ToXnaMatrix(scaleMat * transMat);

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                for (var i = 0; i < 11; ++i)
                {
                    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Vertices, 0, Vertices.Length, Indices, i * 13, 12);
                }
            }
        }
    }
}
