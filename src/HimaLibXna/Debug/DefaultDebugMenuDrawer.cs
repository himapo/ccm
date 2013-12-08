using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Debug
{
    public class DefaultDebugMenuDrawer : IDebugMenuDrawer
    {
        public void Draw(string label, List<DebugMenuNode> nodes, int selected)
        {
            DrawFont(label, 140.0f, 110.0f, Color.LightBlue, new Color(0.0f, 0.0f, 0.0f, 0.2f));

            for (var i =0; i<nodes.Count; ++i)
            {
                var FontColor = (i == selected) ? Color.Red : Color.White;
                var BGColor = (i == selected)
                    ? (new Color(1.0f, 1.0f, 0.0f, 0.2f))
                    : (new Color(0.0f, 0.0f, 0.0f, 0.2f));

                DrawFont(nodes[i].Label, 160.0f, 140.0f + 25.0f * i, FontColor, BGColor);
            }
        }

        void DrawFont(string output, float x, float y, Color fontColor, Color bgColor)
        {
            DebugFontBase.Instance.Draw(output, x, y, fontColor, bgColor);
        }
    }
}
