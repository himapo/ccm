using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class BillboardXna : IBillboard
    {
        public BillboardXna()
        {
        }

        public void Render(BillboardRenderParameter param)
        {
            var renderer = BillboardRendererFactoryXna.Instance.Create(param);
            renderer.Render();
        }
    }
}
