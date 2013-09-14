using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public class RenderPath : IRenderPath
    {
        public string Name { get; set; }

        public int RenderTargetIndex { get; set; }

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public List<ModelInfo> ModelInfoList { get; set; }

        public List<BillboardInfo> BillboardInfoList { get; set; }

        public IRenderDevice RenderDevice { get; set; }

        public bool DepthSortEnabled { get; set; }

        public bool DepthTestEnabled { get; set; }

        public bool DepthWriteEnabled { get; set; }

        public bool DepthClearEnabled { get; set; }

        public bool RenderModelEnabled { get; set; }

        public bool RenderShadowModelOnly { get; set; }

        public bool RenderOpaqueModelOnly { get; set; }

        public bool RenderTranslucentModelOnly { get; set; }

        public bool RenderBillboardEnabled { get; set; }

        public bool RenderShadowBillboardOnly { get; set; }

        public bool RenderOpaqueBillboardOnly { get; set; }

        public bool RenderTranslucentBillboardOnly { get; set; }

        public bool RenderNoHudBillboardOnly { get; set; }

        public bool RenderHudBillboardOnly { get; set; }

        public RenderPath()
        {
            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = false;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = false;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = false;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = false;
            RenderHudBillboardOnly = false;
        }

        public virtual void Render()
        {
            RenderDevice.SetRenderTarget(RenderTargetIndex);

            if (DepthSortEnabled)
            {
            }

            RenderDevice.SetDepthState(DepthTestEnabled, DepthWriteEnabled);

            if (DepthClearEnabled)
            {
                RenderDevice.ClearDepth();
            }

            if (RenderModelEnabled)
            {
                foreach (var info in ModelInfoList)
                {
                    if (RenderShadowModelOnly && !info.RenderParam.ShadowEnabled)
                    {
                        continue;
                    }

                    if (RenderOpaqueModelOnly && info.RenderParam.IsTranslucent)
                    {
                        continue;
                    }

                    if (RenderTranslucentModelOnly && !info.RenderParam.IsTranslucent)
                    {
                        continue;
                    }

                    info.RenderParam.Camera = Camera;
                    info.RenderParam.DirectionalLights = DirectionalLights;
                    info.Model.Render(info.RenderParam);
                }
            }

            if (RenderBillboardEnabled)
            {
                foreach (var info in BillboardInfoList)
                {
                    if (RenderShadowBillboardOnly && !info.RenderParam.ShadowEnabled)
                    {
                        continue;
                    }

                    if (RenderOpaqueBillboardOnly && info.RenderParam.IsTranslucent)
                    {
                        continue;
                    }

                    if (RenderTranslucentBillboardOnly && !info.RenderParam.IsTranslucent)
                    {
                        continue;
                    }

                    if (RenderNoHudBillboardOnly && info.RenderParam.IsHud)
                    {
                        continue;
                    }

                    if (RenderHudBillboardOnly && !info.RenderParam.IsHud)
                    {
                        continue;
                    }

                    info.RenderParam.Camera = Camera;
                    info.RenderParam.DirectionalLights = DirectionalLights;
                    info.Billboard.Render(info.RenderParam);
                }
            }
        }
    }
}
