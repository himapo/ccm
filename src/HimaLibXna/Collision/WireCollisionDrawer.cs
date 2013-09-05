using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;
using HimaLib.System;
using HimaLib.Camera;

namespace HimaLib.Collision
{
    public class WireCollisionDrawer : ICollisionDrawer
    {
        public CameraBase Camera { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        BasicEffect basicEffect;

        VertexPositionColor[] sphereVertices;
        
        short[] sphereIndices;

        readonly int CylinderDivision = 12;

        VertexPositionColor[] cylinderVertices;

        short[] cylinderIndices;

        public WireCollisionDrawer(CameraBase camera)
        {
            Camera = camera;
            basicEffect = new BasicEffect(GraphicsDevice);
            InitEffect();
            InitSphere();
            InitCylinder();
        }

        void InitEffect()
        {
            basicEffect.VertexColorEnabled = true;
        }

        void InitSphere()
        {
            // 球の頂点初期化
            sphereVertices = new VertexPositionColor[12 * 5 + 2];

            for (var i = 0; i < 5; ++i)
            {
                var theta = MathUtil.ToRadians(30.0f * (i + 1));
                for (var j = 0; j < 12; ++j)
                {
                    var phi = MathUtil.ToRadians(30.0f * j);
                    sphereVertices[i * 12 + j].Position = new Microsoft.Xna.Framework.Vector3(
                        MathUtil.Sin(theta) * MathUtil.Sin(phi),
                        MathUtil.Cos(theta),
                        MathUtil.Sin(theta) * MathUtil.Cos(phi));
                    sphereVertices[i * 12 + j].Color = Microsoft.Xna.Framework.Color.Red;
                }
            }
            sphereVertices[60].Position = Microsoft.Xna.Framework.Vector3.UnitY;
            sphereVertices[60].Color = Microsoft.Xna.Framework.Color.Red;
            sphereVertices[61].Position = -Microsoft.Xna.Framework.Vector3.UnitY;
            sphereVertices[61].Color = Microsoft.Xna.Framework.Color.Red;

            sphereIndices = new short[13 * (6 + 5)];

            // 垂直に6枚の円
            for (var i = 0; i < 6; ++i)
            {
                sphereIndices[i * 13 + 0] = 60;
                sphereIndices[i * 13 + 6] = 61;
                sphereIndices[i * 13 + 12] = 60;
                for (var j = 0; j < 5; ++j)
                {
                    sphereIndices[i * 13 + j + 1] = (short)(j * 12 + i);
                    sphereIndices[i * 13 + 11 - j] = (short)(j * 12 + i + 6);
                }
            }

            // 水平に5枚の円
            var offset = 13 * 6;
            for (var i = 0; i < 5; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    sphereIndices[i * 13 + j + offset] = (short)(i * 12 + j);
                }
                sphereIndices[i * 13 + 12 + offset] = sphereIndices[i * 13 + offset];
            }
        }

        void InitCylinder()
        {
            // 円柱の頂点初期化
            cylinderVertices = new VertexPositionColor[CylinderDivision * 2];

            var piover6 = MathUtil.Pi / 6;
            for (var i = 0; i < CylinderDivision * 2; ++i)
            {
                cylinderVertices[i].Position = new Microsoft.Xna.Framework.Vector3(
                    (float)global::System.Math.Cos(piover6 * (i % CylinderDivision)),
                    (i < CylinderDivision) ? 0.0f : 1.0f,
                    (float)global::System.Math.Sin(piover6 * (i % CylinderDivision)));
                cylinderVertices[i].Color = Microsoft.Xna.Framework.Color.Purple;
            }

            // 縦ライン用のインデックス
            cylinderIndices = new short[CylinderDivision * 2];
            for (var i = 0; i < CylinderDivision; ++i)
            {
                cylinderIndices[i * 2 + 0] = (short)i;
                cylinderIndices[i * 2 + 1] = (short)(i + CylinderDivision);
            }
        }

        public void DrawSphere(SphereCollisionPrimitive primitive, bool active)
        {
            UpdateCamera();

            var scaleMat = Matrix.CreateScale(primitive.Radius());
            var transMat = Matrix.CreateTranslation(primitive.Center());
            basicEffect.World = MathUtilXna.ToXnaMatrix(scaleMat * transMat);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                for (var i = 0; i < 11; ++i)
                {
                    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, sphereVertices, 0, sphereVertices.Length, sphereIndices, i * 13, 12);
                }
            }
        }

        public void DrawCylinder(CylinderCollisionPrimitive primitive, bool active)
        {
            UpdateCamera();

            var scaleMat = Matrix.CreateScale(
                    primitive.Radius(),
                    primitive.Height(),
                    primitive.Radius());
            var transMat = Matrix.CreateTranslation(primitive.Base());
            basicEffect.World = MathUtilXna.ToXnaMatrix(scaleMat * transMat);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, cylinderVertices, 0, CylinderDivision - 1);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, cylinderVertices, CylinderDivision, CylinderDivision - 1);

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, cylinderVertices, 0, cylinderVertices.Length, cylinderIndices, 0, CylinderDivision);
            }
        }

        void UpdateCamera()
        {
            basicEffect.View = MathUtilXna.ToXnaMatrix(Camera.View);
            basicEffect.Projection = MathUtilXna.ToXnaMatrix(Camera.Projection);
        }
    }
}
