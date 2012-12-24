using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public enum ModelRendererType
    {
        Simple,
        Toon,
    }

    public interface IModelRenderParameter
    {
        ModelRendererType Type { get; }
    }
}
