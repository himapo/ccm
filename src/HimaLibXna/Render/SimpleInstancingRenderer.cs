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
    public class SimpleInstancingRenderer
    {
        public string ModelName { get; set; }

        public List<AffineTransform> Transforms { get; set; }

        public ICamera Camera { get; set; }

        ModelLoader modelLoader;

        InstancingPhongShader shader;

        public SimpleInstancingRenderer()
        {
            modelLoader = new ModelLoader();
            shader = new InstancingPhongShader();
        }

        public void Render()
        {
            shader.Model = GetModel();
            shader.ModelBones = GetModelBones(shader.Model);
            shader.InstanceTransforms = GetInstanceTransforms();
            shader.View = GetViewMatrix();
            shader.Projection = GetProjMatrix();

            shader.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(0.3f, 0.3f, 0.3f);
            shader.DirLight0Direction = -Microsoft.Xna.Framework.Vector3.One;
            shader.DirLight0Direction.Normalize();
            shader.DirLight0DiffuseColor = new Microsoft.Xna.Framework.Vector3(0.5f, 0.6f, 0.8f);
            shader.DirLight0SpecularColor = Microsoft.Xna.Framework.Vector3.One;
            shader.EyePosition = Vector3.CreateXnaVector(Camera.Eye);

            shader.RenderModel();
        }

        Microsoft.Xna.Framework.Graphics.Model GetModel()
        {
            return modelLoader.Load(ModelName);
        }

        Microsoft.Xna.Framework.Matrix[] GetModelBones(Microsoft.Xna.Framework.Graphics.Model model)
        {
            var modelBones = new Microsoft.Xna.Framework.Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelBones);
            return modelBones;
        }

        Microsoft.Xna.Framework.Matrix[] GetInstanceTransforms()
        {
            var result = new Microsoft.Xna.Framework.Matrix[Transforms.Count];
            for (var i = 0; i < Transforms.Count; ++i)
            {
                result[i] = Matrix.CreateXnaMatrix(Transforms[i].WorldMatrix);
            }
            return result;
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
