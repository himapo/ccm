using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class NullModelRendererXna : IModelRendererXna
    {
        public void SetParameter(ModelRenderParameter param)
        {
        }

        public void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }

        public void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model)
        {
        }
    }
}
