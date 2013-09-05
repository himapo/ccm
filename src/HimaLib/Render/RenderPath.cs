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

        public CameraBase Camera { get; set; }

        public List<DirectionalLight> DirectionalLights { get; set; }

        public List<ModelInfo> ModelInfoList { get; set; }

        public List<BillboardInfo> BillboardInfoList { get; set; }

        public IRenderDevice RenderDevice { get; set; }

        public bool DepthSortEnabled { get; set; }

        public bool DepthTestEnabled { get; set; }

        public bool DepthWriteEnabled { get; set; }

        public bool DepthClearEnabled { get; set; }

        public RenderPath()
        {
            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = false;
        }

        public void Render()
        {
            if (DepthSortEnabled)
            {
            }

            RenderDevice.SetDepthState(DepthTestEnabled, DepthWriteEnabled);

            if (DepthClearEnabled)
            {
                RenderDevice.ClearDepth();
            }

            foreach (var info in ModelInfoList)
            {
                info.RenderParam.Camera = Camera;
                info.RenderParam.DirectionalLights = DirectionalLights;
                info.Model.Render(info.RenderParam);
            }

            foreach (var info in BillboardInfoList)
            {
                info.RenderParam.Camera = Camera;
                info.RenderParam.DirectionalLights = DirectionalLights;
                info.Billboard.Render(info.RenderParam);
            }
        }
    }
}
