﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.Camera;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class SimpleModelRendererXna : IModelRendererXna
    {
        OpaqueFinalShader lambert = new OpaqueFinalShader();

        public SimpleModelRendererXna()
        {
        }

        public void SetParameter(ModelRenderParameter p)
        {
            var param = p as SimpleModelRenderParameter;
            if (param == null)
            {
                return;
            }

            lambert.World = MathUtilXna.ToXnaMatrix(param.Transform);
            lambert.View = MathUtilXna.ToXnaMatrix(param.Camera.View);
            lambert.Projection = MathUtilXna.ToXnaMatrix(param.Camera.Projection);
            
            lambert.Alpha = param.Alpha;
            lambert.AmbientLightColor = MathUtilXna.ToXnaVector(param.AmbientLightColor);

            lambert.ShadowEnabled = param.IsShadowReceiver;
            if (lambert.ShadowEnabled)
            {
                lambert.LightViewProjection = MathUtilXna.ToXnaMatrix(param.LightCamera.View * param.LightCamera.Projection);
                lambert.ShadowMap = (param.ShadowMap as ITextureXna).Texture;
            }

            lambert.DiffuseLightMap = (param.DiffuseLightMap as ITextureXna).Texture;
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
            lambert.Model = model;

            lambert.RenderModel();
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }
    }
}
