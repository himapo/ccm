using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA.Accessory;
using MikuMikuDance.XNA.Model;
using HimaLib.Math;

namespace HimaLib.Render
{
    public interface IModelRendererMMDX
    {
        void Render(MMDXModel model, AffineTransform transform);

        void Render(MMDAccessory model);
    }
}
