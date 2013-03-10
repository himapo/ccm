using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public interface IBillboard
    {
        void Render(IBillboardRenderParameter param);
    }
}
