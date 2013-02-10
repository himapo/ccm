using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Graphics;
using HimaLib.Math;
using HimaLib.System;
using HimaLib.Camera;

namespace HimaLib.Collision
{
    public class WireCollisionDrawer : GraphicsDeviceUser, ICollisionDrawer
    {
        public ICamera Camera { get; set; }

        BasicEffect basicEffect = new BasicEffect(GraphicsDevice);

        VertexPositionColor[] sphereVertices;
        
        short[] sphereIndices;

        readonly int CylinderDivision = 12;

        VertexPositionColor[] cylinderVertices;

        short[] cylinderIndices;

        public WireCollisionDrawer(ICamera camera)
        {
            Camera = camera;
            InitEffect();
            InitCylinder();
        }

        void InitEffect()
        {
            basicEffect.VertexColorEnabled = true;
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
            basicEffect.View = CameraUtil.GetViewMatrix(Camera);
            basicEffect.Projection = CameraUtil.GetProjMatrix(Camera);
        }
    }
}
