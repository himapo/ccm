using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface ICylinderRendererXna
    {
        void SetParameter(CylinderRenderParameter param);

        void Render(CylinderXna cylinder);
    }
}
