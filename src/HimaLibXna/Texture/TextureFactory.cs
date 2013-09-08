using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Texture
{
    public class TextureFactory
    {
        public static TextureFactory Instance
        {
            get
            {
                return Singleton<TextureFactory>.Instance;
            }
        }

        TextureFactory()
        {
        }

        public ITexture CreateFromImage(string name)
        {
            return new ImageTexture(name);
        }

        public ITexture CreateRenderTarget(int index)
        {
            return new RenderTargetTexture(index);
        }
    }
}
