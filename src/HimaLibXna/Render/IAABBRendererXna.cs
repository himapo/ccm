using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public interface IAABBRendererXna
    {
        void SetParameter(AABBRenderParameter param);

        void Render(AABBXna aabb);
    }
}
