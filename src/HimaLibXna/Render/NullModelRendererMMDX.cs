using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA.Model;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class NullModelRendererMMDX : IModelRendererMMDX
    {
        public void Render(MMDXModel model, AffineTransform transform)
        {
        }
    }
}
