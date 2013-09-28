﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class DeferredRenderer : ScreenBillboardRenderer
    {
        DeferredShader Shader = new DeferredShader();

        public DeferredRenderer()
        {
        }

        public override void SetParameter(BillboardRenderParameter p)
        {
            var param = p as DeferredBillboardRenderParameter;
            if (param == null)
            {
                return;
            }

            Shader.World = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            Shader.View = MathUtilXna.ToXnaMatrix(Matrix.Identity);
            Shader.Projection = MathUtilXna.ToXnaMatrix(GetProjMatrix());

            Shader.DirLight0Direction = MathUtilXna.ToXnaVector(param.DirectionalLight.Direction);
            Shader.DirLight0DiffuseColor = MathUtilXna.ToXnaColor(param.DirectionalLight.Color).ToVector3();

            Shader.AlbedoMap = (param.AlbedoMap as ITextureXna).Texture;
            Shader.PositionMap = (param.PositionMap as ITextureXna).Texture;
            Shader.NormalDepthMap = (param.NormalDepthMap as ITextureXna).Texture;
        }

        public override void Render()
        {
            Shader.SetRenderTargetSize(ScreenWidth, ScreenHeight);
            Shader.RenderBillboard();
        }
    }
}