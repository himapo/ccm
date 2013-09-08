using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Texture
{
    public class RenderTargetTexture : ITextureXna, ITexture
    {
        public int Index { get; private set; }

        public Texture2D Texture
        {
            get
            {
                if (RenderTarget == null)
                {
                    CreateRenderTarget();
                }
                return RenderTarget;
            }
        }

        RenderTarget2D RenderTarget;

        public RenderTargetTexture(int index)
        {
            Index = index;
        }

        public void CreateRenderTarget()
        {
            RenderTarget = RenderTargetManager.Instance.GetRenderTarget(Index, 1024, 1024);
        }
    }
}
