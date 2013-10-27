using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class AABBXna : AABB
    {
        public override void Render(AABBRenderParameter param)
        {
            AABBRendererFactoryXna.Instance.Create(param).Render(this);
        }
    }
}
