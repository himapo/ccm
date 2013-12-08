using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public enum FontRendererType
    {
        Sprite,
    }

    public class FontRenderParameter : RenderParameter
    {
        public FontRendererType Type { get; set; }

        public string FontName { get; set; }

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }

        public Color BGColor { get; set; }
    }
}
