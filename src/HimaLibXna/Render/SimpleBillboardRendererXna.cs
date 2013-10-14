using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Content;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class SimpleBillboardRendererXna : IBillboardRendererXna
    {
        ConstantShader ConstantShader = new ConstantShader();

        public SimpleBillboardRendererXna()
        {
        }

        public void SetParameter(BillboardRenderParameter p)
        {
            var param = p as SimpleBillboardRenderParameter;
            if (param == null)
            {
                return;
            }

            ConstantShader.World = MathUtilXna.ToXnaMatrix(CalcWorldMatrix(param.AffineTransform, param.Camera));
            
            ConstantShader.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            ConstantShader.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            ConstantShader.Alpha = param.Alpha;
            ConstantShader.Texture = (param.Texture as ITextureXna).Texture;
        }

        Matrix CalcWorldMatrix(AffineTransform transform, CameraBase camera)
        {
            var scale = Matrix.CreateScale(transform.Scale);
            var rotat =
                Matrix.CreateRotationZ(transform.Rotation.Z) *
                Matrix.CreateRotationY(transform.Rotation.Y) *
                Matrix.CreateRotationX(transform.Rotation.X) *
                CalcBillboardRotateMatrix(camera);
            var trans = Matrix.CreateTranslation(transform.Translation);

            return scale * rotat * trans;
        }

        /// <summary>
        /// 視線と逆を向く回転行列作成
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        Matrix CalcBillboardRotateMatrix(CameraBase camera)
        {
            var rotatMatrix = Matrix.CreateLookAt(
                Vector3.Zero,
                camera.At - camera.Eye,
                camera.Up);
            return Matrix.Invert(rotatMatrix);
        }

        public void Render()
        {
            ConstantShader.RenderBillboard();
        }
    }
}
