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
    public class SimpleModelRenderer
    {
        public string ModelName { get; set; }

        public AffineTransform Transform { get; set; }

        public ICamera Camera { get; set; }

        ModelLoader modelLoader = new ModelLoader();

        LambertShader lambert = new LambertShader();

        public SimpleModelRenderer()
        {
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

        Microsoft.Xna.Framework.Graphics.Model GetModel()
        {
            return modelLoader.Load(ModelName);
        }

        Microsoft.Xna.Framework.Matrix GetWorldMatrix()
        {
            return Matrix.CreateXnaMatrix(Transform.WorldMatrix);
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
