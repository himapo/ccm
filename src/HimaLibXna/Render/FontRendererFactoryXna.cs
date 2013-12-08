using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class FontRendererFactoryXna
    {
        static readonly FontRendererFactoryXna instance = new FontRendererFactoryXna();

        public static FontRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<FontRendererType, IFontRendererXna> RendererDic = new Dictionary<FontRendererType, IFontRendererXna>();

        FontRendererFactoryXna()
        {
        }

        public IFontRendererXna Create(FontRendererType type)
        {
            switch (type)
            {
                case FontRendererType.Sprite:
                    return Create<SpriteFontRenderer>(type);
                default:
                    break;
            }

            return new NullFontRendererXna();
        }

        IFontRendererXna Create<RendererType>(FontRendererType type)
            where RendererType : IFontRendererXna, new()
        {
            IFontRendererXna result;
            if (!RendererDic.TryGetValue(type, out result))
            {
                result = new RendererType();
                RendererDic[type] = result;
            }
            return result;
        }
    }
}
