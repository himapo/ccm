﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Render
{
    public class RenderDeviceFactory
    {
        public static RenderDeviceFactory Instance
        {
            get
            {
                return Singleton<RenderDeviceFactory>.Instance;
            }
        }

        RenderDeviceXna RenderDevice = new RenderDeviceXna();

        RenderDeviceFactory()
        {
        }

        public IRenderDevice Create()
        {
            return RenderDevice;
        }
    }
}
