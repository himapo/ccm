using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Render
{
    public class RenderPathFactory
    {
        public static RenderPathFactory Instance
        {
            get
            {
                return Singleton<RenderPathFactory>.Instance;
            }
        }

        RenderPathFactory()
        {
        }

        public IRenderPath CreatePath(string name)
        {
            return new RenderPath()
            {
                Name = name,
                RenderDevice = new RenderDeviceXna(),
            };
        }

        public IRenderPath CreatePath(
            string name,
            bool depthSortEnabled,
            bool depthTestEnabled,
            bool depthWriteEnabled,
            bool depthClearEnabled)
        {
            return new RenderPath()
            {
                Name = name,
                RenderDevice = new RenderDeviceXna(),
                DepthSortEnabled = depthSortEnabled,
                DepthTestEnabled = depthTestEnabled,
                DepthWriteEnabled = depthWriteEnabled,
                DepthClearEnabled = depthClearEnabled,
            };
        }
    }
}
