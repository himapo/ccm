using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public enum AABBRendererType
    {
        Wire,
    }

    public class AABBRenderParameter : RenderParameter
    {
        public AABBRendererType Type { get; set; }

        public Color Color { get; set; }
    }
}
