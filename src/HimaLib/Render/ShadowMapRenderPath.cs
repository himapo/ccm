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
            ColorClearEnabled = true;
            ClearColor = Color.White;

            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = true;

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

            // カメラを退避
            var eyeCamera = Camera;

            // ライトをカメラに変換
            Camera = LightToCamera(DirectionalLights[0], eyeCamera);

            CreateModelList();
            CreateBillboardList();

            base.Render();

            // カメラを戻す
            Camera = eyeCamera;
        }

        CameraBase LightToCamera(DirectionalLight light, CameraBase eyeCamera)
        {
            return light.ToCamera(eyeCamera);
        }

        void CreateModelList()
        {
            var casters = ModelInfoList.Where(info =>
            {
                if (info.RenderParam.IsShadowReceiver)
                {
                    // 光源カメラをレンダーパラメータに渡す
                    info.RenderParam.LightCamera = Camera;
                }

                // 影描画が有効なものを抽出
                if (!info.RenderParam.IsShadowCaster)
                {
                    return false;
                }

                return true;
            }).Select(
            info =>
            {
                var depthParam = new DepthModelRenderParameter()
                {
                    Transform = info.RenderParam.Transform
                };

                // 深度レンダリング用のModelInfoを新たに生成する
                return new ModelInfo()
                {
                    Model = info.Model,
                    RenderParam = depthParam,
                };
            });

            ModelInfoList = casters;
        }

        void CreateBillboardList()
        {
            var casters = BillboardInfoList.Where(info =>
            {
                if (info.RenderParam.IsShadowReceiver)
                {
                    // 光源カメラをレンダーパラメータに渡す
                    info.RenderParam.LightCamera = Camera;
                }

                // 影描画が有効なものを抽出
                if (!info.RenderParam.IsShadowCaster)
                {
                    return false;
                }

                return true;
            }).Select(
            info =>
            {
                var depthParam = new DepthBillboardRenderParameter()
                {
                    Transform = info.RenderParam.Transform
                };

                // 深度レンダリング用のModelInfoを新たに生成する
                return new BillboardInfo()
                {
                    Billboard = info.Billboard,
                    RenderParam = depthParam,
                };
            });

            BillboardInfoList = casters;
        }
    }
}
