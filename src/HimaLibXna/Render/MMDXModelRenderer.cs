using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.Core;
using MikuMikuDance.Core.Model;
using MikuMikuDance.XNA;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Content;

namespace HimaLib.Render
{
    public class MMDXModelRenderer
    {
        public AffineTransform Transform { get; set; }

        public ICamera Camera { get; set; }

        MMDModel model;

        MMDXModelLoader modelLoader = new MMDXModelLoader();

        public MMDXModelRenderer()
        {
        }

        public void SetUp(string modelName)
        {
            model = modelLoader.Load(modelName);
        }

        public void Render()
        {
            model.Transform = Matrix.CreateXnaMatrix(Transform.WorldMatrix);

            SetCameraParameter();

            model.Draw();
        }

        void SetCameraParameter()
        {
            MMDXCore.Instance.Camera.Position = Vector3.CreateXnaVector(Camera.Eye);
            MMDXCore.Instance.Camera.SetVector(Vector3.CreateXnaVector(Camera.At) - Vector3.CreateXnaVector(Camera.Eye));
            MMDXCore.Instance.Camera.FieldOfView = MathUtil.ToRadians(Camera.FovY);
            MMDXCore.Instance.Camera.Near = Camera.Near;
            MMDXCore.Instance.Camera.Far = Camera.Far;
        }
    }
}
