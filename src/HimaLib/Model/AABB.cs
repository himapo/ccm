using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;

namespace HimaLib.Model
{
    public abstract class AABB
    {
        public Vector3 Corner { get; set; }

        public Vector3 Width { get; set; }

        public abstract void Render(AABBRenderParameter param);
    }
}
