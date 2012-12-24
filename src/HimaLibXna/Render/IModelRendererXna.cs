using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public interface IModelRendererXna
    {
        void Render(Microsoft.Xna.Framework.Graphics.Model model, AffineTransform transform);
    }
}
