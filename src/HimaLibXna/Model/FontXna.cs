using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class FontXna : Font
    {
        public override void Render(Render.FontRenderParameter param)
        {
            var renderer = FontRendererFactoryXna.Instance.Create(param.Type);
            renderer.SetParameter(param);
            renderer.Render(this);
        }
    }
}
