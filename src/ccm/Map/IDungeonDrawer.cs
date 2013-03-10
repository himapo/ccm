using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Map
{
    public interface IDungeonDrawer
    {
        void DrawMapCube(IModel model, bool updated, List<AffineTransform> transforms);
    }
}
