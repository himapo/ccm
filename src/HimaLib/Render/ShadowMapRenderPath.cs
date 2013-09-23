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
        CameraBase EyeCamera;

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

            if (EyeCamera == null)
            {
                EyeCamera = Camera;
            }

            // TODO : ライトをカメラに変換
            Camera = LightToCamera(DirectionalLights[0]);

            var casters = ModelInfoList.Where(info =>
            {
                // 影描画が有効なものを抽出
                if (!(info.RenderParam is ShadowModelRenderParameter))
                {
                    return false;
                }

                var shadowParam = info.RenderParam as ShadowModelRenderParameter;
                
                if (info.RenderParam.IsShadowReceiver)
                {
                    // 光源カメラをレンダーパラメータに渡す
                    shadowParam.LightCamera = Camera;
                }

                if (!info.RenderParam.IsShadowCaster)
                {
                    return false;
                }

                return true;
            }).Select(
            info =>
            {
                var shadowParam = info.RenderParam as ShadowModelRenderParameter;

                // 深度レンダリング用のModelInfoを新たに生成する
                return new ModelInfo()
                {
                    Model = info.Model,
                    RenderParam = shadowParam.DepthModelRenderParameter,
                };
            });

            ModelInfoList = casters;

            base.Render();
        }

        CameraBase LightToCamera(DirectionalLight light)
        {
            return light.ToCamera(EyeCamera);
        }
    }
}
