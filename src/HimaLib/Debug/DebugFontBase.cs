using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Model;

namespace HimaLib.Debug
{
    public abstract class DebugFontBase
    {
        public static DebugFontBase Instance { get; protected set; }

        public RenderScene RenderScene { get; set; }

        public string FontName { get; set; }

        protected DebugFontBase()
        {
        }

        public void Draw(string output, float x, float y)
        {
            Draw(output, x, y, Color.White, new Color(Vector4.Zero));
        }

        public void Draw(string output, float x, float y, Color fontColor, Color bgColor)
        {
#if DEBUG
            var renderParam = new FontRenderParameter()
            {
                Type = FontRendererType.Sprite,
                FontName = this.FontName,
                Position = new Vector2(x, y),
                FontColor = fontColor,
                BGColor = bgColor,
            };

            RenderScene.RenderFont(CreateFont(output), renderParam);
#endif
        }

        protected abstract Font CreateFont(string output);
    }
}
