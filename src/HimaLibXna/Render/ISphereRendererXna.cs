using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface ISphereRendererXna
    {
        void SetParameter(SphereRenderParameter param);

        void Render(Sphere sphere);
    }
}
