using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class SphereXna : Sphere
    {
        public SphereXna()
        {

        }

        public override void Render(Render.SphereRenderParameter param)
        {
            SphereRendererFactoryXna.Instance.Create(param).Render(this);
        }
    }
}
