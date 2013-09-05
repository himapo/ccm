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

        public virtual void Render()
        {
            if (DepthSortEnabled)
            {
            }

            RenderDevice.SetDepthState(DepthTestEnabled, DepthWriteEnabled);

            if (DepthClearEnabled)
            {
                RenderDevice.ClearDepth();
            }

            foreach (var modelInfo in ModelInfoList)
            {
                modelInfo.RenderParam.Camera = Camera;
                modelInfo.RenderParam.DirectionalLights = DirectionalLights;
                modelInfo.Model.Render(modelInfo.RenderParam);
            }
        }

        //protected abstract void RenderDetail();
    }
}
