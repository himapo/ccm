using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA;
using MikuMikuDance.XNA.Model;
using MikuMikuDance.XNA.Accessory;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class ToonModelRendererMMDX : IModelRendererMMDX
    {
        AffineTransform Transform;

        public void SetParameter(ToonModelRenderParameter param)
        {
            MMDXCore.Instance.Camera.Position = MathUtilXna.ToXnaVector(param.Camera.Eye);
            MMDXCore.Instance.Camera.SetVector(MathUtilXna.ToXnaVector(param.Camera.At) - MathUtilXna.ToXnaVector(param.Camera.Eye));
            MMDXCore.Instance.Camera.FieldOfView = MathUtil.ToRadians(param.Camera.FovY);
            MMDXCore.Instance.Camera.Near = param.Camera.Near;
            MMDXCore.Instance.Camera.Far = param.Camera.Far;

            Transform = param.Transform;
        }

        public void Render(MMDXModel model)
        {
            model.Transform = MathUtilXna.ToXnaMatrix(Transform.WorldMatrix);
            model.Draw();
        }

        public void Render(MMDAccessory model)
        {
            model.Draw();
        }
    }
}
