using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class ShadowMapRenderPath : RenderPath
    {
        public ShadowMapRenderPath()
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
            if (DirectionalLights.Count == 0)
            {
                return;
            }

            // TODO : ライトをカメラに変換
            Camera = LightToCamera(DirectionalLights[0]);

            var casters = ModelInfoList.Where(info =>
            {
                if (!info.RenderParam.ShadowEnabled)
                {
                    return false;
                }

                if (info.RenderParam.IsTranslucent)
                {
                    return false;
                }

                if (info.RenderParam.ShadowMapRenderParameter == null)
                {
                    return false;
                }

                return true;
            }).Select(
            info => new ModelInfo()
            {
                Model = info.Model,
                RenderParam = info.RenderParam.ShadowMapRenderParameter,
            });

            ModelInfoList = casters;

            base.Render();
        }

        CameraBase LightToCamera(DirectionalLight light)
        {
            return new CameraBase()
            {
                Eye = Vector3.One * 50.0f,
                At = Vector3.Zero,
                Up = Vector3.Up,
                Near = 10.0f,
                Far = 1000.0f,
            };
        }
    }
}
