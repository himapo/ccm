using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    enum DebugFontArea
    {
        LeftTop,
        LeftCenter,
        LeftBottom,
        RightTop,
        RightCenter,
        RightBottom,
    }

    class DebugFontInfo
    {
        public string Output { get; set; }
        public Vector2 Position { get; set; }
        public Color FontColor { get; set; }
        public DebugFontArea Area { get; set; }
        public Color BGColor { get; set; }

        public DebugFontInfo()
        {
            Output = "";
            Position = new Vector2(0.0f, 0.0f);
            FontColor = Color.White;
            BGColor = Color.Transparent;
        }

        public DebugFontInfo(string output, Vector2 position, Color fontColor, Color bgColor)
        {
            Initialize(output, position, fontColor, bgColor);
        }

        public DebugFontInfo(string output, Vector2 position)
        {
            Initialize(output, position, Color.White, Color.Transparent);
        }

        void Initialize(string output, Vector2 position, Color fontColor, Color bgColor)
        {
            Output = output;
            Position = position;
            FontColor = fontColor;
            BGColor = bgColor;
        }
    }
}
