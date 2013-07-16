using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
