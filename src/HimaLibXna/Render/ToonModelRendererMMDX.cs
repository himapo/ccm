using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA;
using MikuMikuDance.XNA.Model;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class ToonModelRendererMMDX : IModelRendererMMDX
    {
        public void SetParameter(ToonModelRenderParameter param)
        {
            MMDXCore.Instance.Camera.Position = Vector3.CreateXnaVector(param.Camera.Eye);
            MMDXCore.Instance.Camera.SetVector(Vector3.CreateXnaVector(param.Camera.At) - Vector3.CreateXnaVector(param.Camera.Eye));
            MMDXCore.Instance.Camera.FieldOfView = MathUtil.ToRadians(param.Camera.FovY);
            MMDXCore.Instance.Camera.Near = param.Camera.Near;
            MMDXCore.Instance.Camera.Far = param.Camera.Far;
        }

        public void Render(MMDXModel model, AffineTransform transform)
        {
            model.Transform = Matrix.CreateXnaMatrix(transform.WorldMatrix);
            model.Draw();
        }
    }
}
