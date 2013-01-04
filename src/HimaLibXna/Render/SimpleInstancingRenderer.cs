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

        Microsoft.Xna.Framework.Graphics.Model model;

        Microsoft.Xna.Framework.Matrix[] modelBones;

        Microsoft.Xna.Framework.Matrix[] instanceTransforms;

        InstancingPhongShader shader;

        public SimpleInstancingRenderer()
        {
            Transforms = new List<AffineTransform>();
            modelLoader = new ModelLoader();
            shader = new InstancingPhongShader();
        }

        public void SetUp()
        {
            SetUpModel();
            SetUpModelBones();
            SetUpInstanceTransforms();
        }

        void SetUpModel()
        {
            model = modelLoader.Load(ModelName);
        }

        void SetUpModelBones()
        {
            Array.Resize(ref modelBones, model.Bones.Count);
            model.CopyAbsoluteBoneTransformsTo(modelBones);
        }

        void SetUpInstanceTransforms()
        {
            Array.Resize(ref instanceTransforms, Transforms.Count);
            for (var i = 0; i < Transforms.Count; ++i)
            {
                instanceTransforms[i] = MathUtilXna.ToXnaMatrix(Transforms[i].WorldMatrix);
            }
        }

        public void Render()
        {
            SetShaderParameters();

            shader.RenderModel();
        }

        void SetShaderParameters()
        {
            shader.Model = model;
            shader.ModelBones = modelBones;
            shader.InstanceTransforms = instanceTransforms;
            shader.View = GetViewMatrix();
            shader.Projection = GetProjMatrix();

            shader.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(0.3f, 0.3f, 0.3f);
            shader.DirLight0Direction = -Microsoft.Xna.Framework.Vector3.One;
            shader.DirLight0Direction.Normalize();
            shader.DirLight0DiffuseColor = new Microsoft.Xna.Framework.Vector3(0.5f, 0.6f, 0.8f);
            shader.DirLight0SpecularColor = Microsoft.Xna.Framework.Vector3.One;
            shader.EyePosition = MathUtilXna.ToXnaVector(Camera.Eye);
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
