using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.Player
{
    public interface IPlayerDrawer
    {
        void Draw(string modelName, List<string> attackmentNames, AffineTransform transform);
    }
}
