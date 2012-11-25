using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Shader;
using HimaLib.Content;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class SimpleModelRenderer
    {
        public string ModelName { get; set; }

        public float Scale { get; set; }

        public HimaLib.Math.Vector3 Rotation { get; set; }

        public HimaLib.Math.Vector3 Position { get; set; }

        public ICamera Camera { get; set; }

        ConstantShader constant;
        LambertShader lambert;

        ModelLoader ModelLoader { get; set; }

        public SimpleModelRenderer()
        {
            Scale = 1.0f;
            Rotation = HimaLib.Math.Vector3.Zero;
            Position = HimaLib.Math.Vector3.Zero;

            constant = new ConstantShader();
            lambert = new LambertShader();
            ModelLoader = new ModelLoader();
        }

        public void Render()
        {
            constant.Model = GetModel();
            constant.World = GetWorldMatrix();
            constant.View = GetViewMatrix();
            constant.Projection = GetProjMatrix();
            constant.Alpha = 1.0f;

            lambert.Model = GetModel();
            lambert.World = GetWorldMatrix();
            lambert.View = GetViewMatrix();
            lambert.Projection = GetProjMatrix();
            lambert.Alpha = 1.0f;

            lambert.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
            lambert.DirLight0Direction = new Vector3(-1.0f, -1.0f, -1.0f);
            lambert.DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
            
            lambert.RenderModel();
        }

        Model GetModel()
        {
            return ModelLoader.Load(ModelName);
        }

        Matrix GetWorldMatrix()
        {
            var result = Matrix.CreateScale(Scale);
            result *= Matrix.CreateRotationX(Rotation.X);
            result *= Matrix.CreateRotationY(Rotation.Y);
            result *= Matrix.CreateRotationZ(Rotation.Z);
            result *= Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);
            return result;
        }

        Matrix GetViewMatrix()
        {
            return CameraUtil.GetViewMatrix(Camera);
        }

        Matrix GetProjMatrix()
        {
            return CameraUtil.GetProjMatrix(Camera);
        }
    }
}
