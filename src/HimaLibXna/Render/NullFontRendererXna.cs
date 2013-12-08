using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public class NullFontRendererXna : IFontRendererXna
    {
        public void SetParameter(FontRenderParameter param) { }

        public void Begin() { }

        public void End() { }

        public void Render(FontXna font) { }
    }
}
