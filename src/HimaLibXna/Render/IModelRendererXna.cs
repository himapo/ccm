using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public interface IModelRendererXna
    {
        void SetParameter(ModelRenderParameter param);

        void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model);

        void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model);
    }
}
