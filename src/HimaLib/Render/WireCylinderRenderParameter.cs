using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class WireCylinderRenderParameter : CylinderRenderParameter
    {
        public override CylinderRendererType Type
        {
            get { return CylinderRendererType.Wire; }
        }

        public Color Color { get; set; }
    }
}
