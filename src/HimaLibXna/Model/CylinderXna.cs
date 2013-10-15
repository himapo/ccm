using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class CylinderXna : Cylinder
    {
        public override void Render(Render.CylinderRenderParameter param)
        {
            CylinderRendererFactoryXna.Instance.Create(param).Render(this);
        }
    }
}
