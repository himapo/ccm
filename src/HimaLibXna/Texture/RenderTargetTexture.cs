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
                return RenderTargetManager.Instance.GetRenderTarget(Index);
            }
        }

        public RenderTargetTexture(int index)
        {
            Index = index;
        }
    }
}
