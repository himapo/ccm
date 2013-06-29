using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public interface IModelRendererXna
    {
        void SetParameter(IModelRenderParameter param);

        void Render(Microsoft.Xna.Framework.Graphics.Model model);
    }
}
