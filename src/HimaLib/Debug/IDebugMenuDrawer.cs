using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public interface IDebugMenuDrawer
    {
        void Draw(string label, List<DebugMenuNode> nodes, int selected);
    }
}
