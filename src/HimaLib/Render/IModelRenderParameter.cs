using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Light;

namespace HimaLib.Render
{
    public enum ModelRendererType
    {
        Default,
        Simple,
        SimpleInstancing,
        Toon,
    }

    public interface IModelRenderParameter
    {
        ModelRendererType Type { get; }

        CameraBase Camera { get; set; }

        List<DirectionalLight> DirectionalLights { get; set; }
    }
}
