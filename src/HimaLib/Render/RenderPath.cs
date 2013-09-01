using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public abstract class RenderPath : IRenderPath
    {
        public string Name { get; set; }

        public ICamera Camera { get; set; }

        public List<ModelInfo> ModelInfoList { get; set; }

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

            SetDepthState(DepthTestEnabled, DepthWriteEnabled);

            if (DepthClearEnabled)
            {
                ClearDepth();
            }

            foreach (var modelInfo in ModelInfoList)
            {
                modelInfo.Model.Render(modelInfo.RenderParam);
            }
        }

        protected abstract void ClearDepth();

        protected abstract void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled);
    }
}
