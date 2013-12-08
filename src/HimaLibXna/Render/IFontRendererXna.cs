using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface IFontRendererXna
    {
        void SetParameter(FontRenderParameter param);

        void Begin();

        void End();

        void Render(FontXna font);
    }
}
