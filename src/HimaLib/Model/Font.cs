using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public abstract class Font
    {
        public string String { get; set; }

        public abstract void Render(FontRenderParameter param);
    }
}
