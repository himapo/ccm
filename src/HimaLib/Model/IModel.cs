using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public interface IModel
    {
        string Name { get; set; }

        void Render(IModelRenderParameter renderer);
    }
}
