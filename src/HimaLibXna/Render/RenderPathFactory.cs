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

        public IRenderPath CreateOpaquePath(string name)
        {
            return new OpaqueRenderPath()
            {
                Name = name,
                RenderDevice = new RenderDeviceXna(),
            };
        }

        public IRenderPath CreateTranslucentPath(string name)
        {
            return new TranslucentRenderPath()
            {
                Name = name,
                RenderDevice = new RenderDeviceXna(),
            };
        }

        public IRenderPath CreateHudPath(string name)
        {
            return new HudRenderPath()
            {
                Name = name,
                RenderDevice = new RenderDeviceXna(),
            };
        }
    }
}
