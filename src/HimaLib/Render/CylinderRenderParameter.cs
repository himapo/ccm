using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public enum CylinderRendererType
    {
        Wire,
    }

    public abstract class CylinderRenderParameter : RenderParameter
    {
        public abstract CylinderRendererType Type { get; }

        public CylinderRenderParameter()
        {

        }
    }
}
