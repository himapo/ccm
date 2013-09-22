using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA;
using MikuMikuDance.XNA.Model;
using MikuMikuDance.XNA.Accessory;
using HimaLib.Math;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class ToonModelRendererMMDX : IModelRendererMMDX
    {
        AffineTransform Transform;

        public void SetParameter(ToonModelRenderParameter param)
        {
            var camera = param.Camera as PerspectiveCamera;
            MMDXCore.Instance.Camera.Position = MathUtilXna.ToXnaVector(camera.Eye);
            MMDXCore.Instance.Camera.SetVector(MathUtilXna.ToXnaVector(camera.At) - MathUtilXna.ToXnaVector(camera.Eye));
            MMDXCore.Instance.Camera.FieldOfView = MathUtil.ToRadians(camera.FovY);
            MMDXCore.Instance.Camera.Near = camera.Near;
            MMDXCore.Instance.Camera.Far = camera.Far;

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
