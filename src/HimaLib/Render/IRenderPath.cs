using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface IRenderPath
    {
        string Name { get; set; }

        ICamera Camera { get; set; }

        List<ModelInfo> ModelInfoList { get; set; }

        bool DepthSortEnabled { get; set; }

        bool DepthTestEnabled { get; set; }

        bool DepthWriteEnabled { get; set; }

        bool DepthClearEnabled { get; set; }

        void Render();
    }
}
