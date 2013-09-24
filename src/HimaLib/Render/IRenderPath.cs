using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface IRenderPath
    {
        string Name { get; set; }

        bool Enabled { get; set; }

        int RenderTargetIndex { get; set; }

        CameraBase Camera { get; set; }

        List<DirectionalLight> DirectionalLights { get; set; }        

        IEnumerable<ModelInfo> ModelInfoList { get; set; }

        List<BillboardInfo> BillboardInfoList { get; set; }

        void Render();
    }
}
