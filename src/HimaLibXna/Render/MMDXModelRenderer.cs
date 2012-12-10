using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.Core;
using MikuMikuDance.Core.Model;
using MikuMikuDance.Core.Accessory;
using MikuMikuDance.XNA;
using MikuMikuDance.XNA.Model;
using MikuMikuDance.XNA.Accessory;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Content;

namespace HimaLib.Render
{
    public class MMDXModelRenderer
    {
        public AffineTransform Transform { get; set; }

        public ICamera Camera { get; set; }

        MMDXModel model;

        List<MMDAccessory> accessoryModels = new List<MMDAccessory>();

        MMDXModelLoader modelLoader = new MMDXModelLoader();

        MMDAccessoryLoader accessoryLoader = new MMDAccessoryLoader();

        MMDVACLoader vacLoader = new MMDVACLoader();

        public MMDXModelRenderer()
        {
        }

        public void SetUp(string modelName, List<string> accessoryNames)
        {
            model = modelLoader.Load("Model/" + modelName);

            accessoryModels.Clear();
            foreach (var name in accessoryNames)
            {
                var accessory = accessoryLoader.Load("Accessory/" + name);
                var vac = vacLoader.Load("Accessory/" + name + "-vac");
                model.BindAccessory(accessory, vac);
                accessoryModels.Add(accessory);
            }
        }

        public void Render()
        {
            model.Transform = Matrix.CreateXnaMatrix(Transform.WorldMatrix);

            SetCameraParameter();

            model.Draw();

            foreach (var accessory in accessoryModels)
            {
                accessory.Draw();
            }
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
