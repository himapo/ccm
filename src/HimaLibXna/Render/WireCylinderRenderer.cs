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
    public class WireCylinderRenderer : ICylinderRendererXna
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        BasicEffect BasicEffect;

        readonly int CylinderDivision = 12;

        VertexPositionColor[] Vertices;

        short[] Indices;

        public WireCylinderRenderer()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            InitEffect();
            InitCylinder();
        }

        void InitEffect()
        {
            BasicEffect.VertexColorEnabled = true;
        }

        void InitCylinder()
        {
            // 円柱の頂点初期化
            Vertices = new VertexPositionColor[CylinderDivision * 2];

            var piover6 = MathUtil.Pi / 6;
            for (var i = 0; i < CylinderDivision * 2; ++i)
            {
                Vertices[i].Position = new Microsoft.Xna.Framework.Vector3(
                    (float)global::System.Math.Cos(piover6 * (i % CylinderDivision)),
                    (i < CylinderDivision) ? 0.0f : 1.0f,
                    (float)global::System.Math.Sin(piover6 * (i % CylinderDivision)));
                Vertices[i].Color = Microsoft.Xna.Framework.Color.Purple;
            }

            // 縦ライン用のインデックス
            Indices = new short[CylinderDivision * 2];
            for (var i = 0; i < CylinderDivision; ++i)
            {
                Indices[i * 2 + 0] = (short)i;
                Indices[i * 2 + 1] = (short)(i + CylinderDivision);
            }
        }

        void SetColor(Microsoft.Xna.Framework.Color color)
        {
            for (var i = 0; i < CylinderDivision * 2; ++i)
            {
                Vertices[i].Color = color;
            }
        }

        public void SetParameter(CylinderRenderParameter p)
        {
            var param = p as WireCylinderRenderParameter;
            if (param == null)
            {
                return;
            }

            BasicEffect.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            BasicEffect.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);

            SetColor(MathUtilXna.ToXnaColor(param.Color));
        }

        public void Render(CylinderXna cylinder)
        {
            var scaleMat = Matrix.CreateScale(
                    cylinder.Radius,
                    cylinder.Height,
                    cylinder.Radius);
            var transMat = Matrix.CreateTranslation(cylinder.Base);
            BasicEffect.World = MathUtilXna.ToXnaMatrix(scaleMat * transMat);

            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Vertices, 0, CylinderDivision - 1);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Vertices, CylinderDivision, CylinderDivision - 1);

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, Vertices, 0, Vertices.Length, Indices, 0, CylinderDivision);
            }
        }
    }
}
