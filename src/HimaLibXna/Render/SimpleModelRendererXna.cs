using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class SimpleModelRendererXna : IModelRendererXna
    {
        LambertShader lambert = new LambertShader();

        public SimpleModelRendererXna()
        {
        }

        public void SetParameter(SimpleModelRenderParameter param)
        {
            lambert.World = MathUtilXna.ToXnaMatrix(param.Transform.WorldMatrix);
            lambert.View = CameraUtil.GetViewMatrix(param.Camera);
            lambert.Projection = CameraUtil.GetProjMatrix(param.Camera);
            lambert.Alpha = param.Alpha;
            lambert.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);
            lambert.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirLight0Direction);
            lambert.DirLight0DiffuseColor = MathUtilXna.ToXnaVector(param.DirLight0DiffuseColor);
        }

        public void Render(Microsoft.Xna.Framework.Graphics.Model model)
        {
            lambert.Model = model;

            lambert.RenderModel();
        }
    }
}
