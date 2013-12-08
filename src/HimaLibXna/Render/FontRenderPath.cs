using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class FontRenderPath : RenderPath
    {
        public FontRenderPath()
        {
            Enabled = true;

            ColorClearEnabled = false;

            DepthSortEnabled = false;
            DepthTestEnabled = false;
            DepthWriteEnabled = false;
            DepthClearEnabled = false;
        }

        public override void Render()
        {
            if (!Enabled)
            {
                return;
            }

            SetRenderTarget();

            SetDepthState();

            ClearTarget();

            BeginRenderFont();

            RenderFont();

            EndRenderFont();
        }

        void BeginRenderFont()
        {
            FontRendererFactoryXna.Instance.Create(FontRendererType.Sprite).Begin();
        }

        void EndRenderFont()
        {
            FontRendererFactoryXna.Instance.Create(FontRendererType.Sprite).End();
        }
    }
}
