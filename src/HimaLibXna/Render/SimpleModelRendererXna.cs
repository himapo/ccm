using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Content;
using HimaLib.Camera;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class SimpleModelRendererXna : IModelRendererXna
    {
        public string ModelName { get; set; }

        public AffineTransform Transform { get; set; }

        public ICamera Camera { get; set; }

        ModelLoader modelLoader = new ModelLoader();

        LambertShader lambert = new LambertShader();

        public SimpleModelRendererXna()
        {
        }

        public void SetParameter(SimpleModelRenderParameter param)
        {
            lambert.View = CameraUtil.GetViewMatrix(param.Camera);
            lambert.Projection = CameraUtil.GetProjMatrix(param.Camera);
            lambert.Alpha = param.Alpha;
            lambert.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);
            lambert.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirLight0Direction);
            lambert.DirLight0DiffuseColor = MathUtilXna.ToXnaVector(param.DirLight0DiffuseColor);
        }

        public void Render()
        {
            lambert.Model = GetModel();
            lambert.World = GetWorldMatrix();
            lambert.View = GetViewMatrix();
            lambert.Projection = GetProjMatrix();
            lambert.Alpha = 1.0f;

            lambert.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(0.4f, 0.4f, 0.4f);
            lambert.DirLight0Direction = new Microsoft.Xna.Framework.Vector3(-1.0f, -1.0f, -1.0f);
            lambert.DirLight0DiffuseColor = new Microsoft.Xna.Framework.Vector3(0.5f, 0.6f, 0.8f);
            
            lambert.RenderModel();
        }

        public void Render(Microsoft.Xna.Framework.Graphics.Model model, AffineTransform transform)
        {
            lambert.Model = model;
            lambert.World = MathUtilXna.ToXnaMatrix(transform.WorldMatrix);

            lambert.RenderModel();
        }

        Microsoft.Xna.Framework.Graphics.Model GetModel()
        {
            return modelLoader.Load(ModelName);
        }

        Microsoft.Xna.Framework.Matrix GetWorldMatrix()
        {
            return MathUtilXna.ToXnaMatrix(Transform.WorldMatrix);
        }

        Microsoft.Xna.Framework.Matrix GetViewMatrix()
        {
            return CameraUtil.GetViewMatrix(Camera);
        }

        Microsoft.Xna.Framework.Matrix GetProjMatrix()
        {
            return CameraUtil.GetProjMatrix(Camera);
        }
    }
}
