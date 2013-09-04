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

        public void SetParameter(IModelRenderParameter p)
        {
            var param = p as SimpleModelRenderParameter;
            if (param == null)
            {
                return;
            }

            lambert.World = MathUtilXna.ToXnaMatrix(param.Transform.WorldMatrix);
            lambert.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            lambert.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            lambert.Alpha = param.Alpha;
            lambert.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);
            lambert.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirectionalLights[0].Direction);
            lambert.DirLight0DiffuseColor = MathUtilXna.ToXnaVector(param.DirectionalLights[0].Color.ToVector3());
        }

        public void Render(Microsoft.Xna.Framework.Graphics.Model model)
        {
            lambert.Model = model;

            lambert.RenderModel();
        }
    }
}
