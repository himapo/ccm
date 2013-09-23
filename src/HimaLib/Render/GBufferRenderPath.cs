﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class GBufferRenderPath : RenderPath
    {
        public GBufferRenderPath()
        {
            ClearEnabled = true;
            ClearColor = Color.White;

            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = false;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = true;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = true;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = true;
            RenderHudBillboardOnly = false;
        }

        public override void Render()
        {
            ModelInfoList = ModelInfoList.Where(
            info =>
            {
                return info.RenderParam is ShadowModelRenderParameter;
            }
            ).Select(
            info =>
            {
                var shadowParam = info.RenderParam as ShadowModelRenderParameter;

                // Gバッファ用のModelInfoを新たに生成する
                var gbufferParam = new GBufferModelRenderParameter()
                {
                    Transform = shadowParam.Transform,
                };

                return new ModelInfo()
                {
                    Model = info.Model,
                    RenderParam = gbufferParam,
                };
            });

            base.Render();
        }
    }
}
