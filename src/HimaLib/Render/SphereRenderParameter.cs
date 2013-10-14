using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public enum SphereRendererType
    {
        Wire,
    }

    public abstract class SphereRenderParameter : RenderParameter
    {
        public abstract SphereRendererType Type { get; }

        public SphereRenderParameter()
        {
            
        }
    }
}
