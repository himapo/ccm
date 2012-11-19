using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DefaultDebugMenuDrawer : IDebugMenuDrawer
    {
        public void Draw(string label, List<DebugMenuNode> nodes, int selected)
        {
            DebugFont.Add(label, 150.0f, 90.0f);

            for (var i =0; i<nodes.Count; ++i)
            {
                // TODO : selectedで色付け

                DebugFont.Add(nodes[i].Label, 160.0f, 120.0f + 25.0f * i);
            }
        }
    }
}
