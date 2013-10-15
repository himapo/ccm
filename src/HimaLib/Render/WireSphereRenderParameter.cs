using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class WireSphereRenderParameter : SphereRenderParameter
    {
        public override SphereRendererType Type
        {
            get { return SphereRendererType.Wire; }
        }
    }
}
